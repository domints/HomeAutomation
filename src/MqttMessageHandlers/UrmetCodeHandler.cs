using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.MqttMessageHandlers
{
    [TopicHandler("urmet/code")]
    public class UrmetCodeHandler : IMqttMessageHandler
    {
        private readonly IBot _bot;
        private readonly IDatabase _db;

        public UrmetCodeHandler(IBot bot, IDatabase database)
        {
            _bot = bot;
            _db = database;
        }

        public async Task Handle(string topic, string message)
        {
            if (int.TryParse(message, out int number))
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (number == 29)
                {
                    await _bot.SendMessage("📟 Ktoś użył kodu!");
                }

                await _db.WorkAsync(async (conn) =>
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO calls VALUES ($number, $timestamp, 'code', 0)";
                    cmd.Parameters.AddWithValue("$number", number).DbType = System.Data.DbType.Int16;
                    cmd.Parameters.AddWithValue("$timestamp", timestamp).DbType = System.Data.DbType.Int64;
                    await cmd.ExecuteNonQueryAsync();
                });
            }
        }
    }
}