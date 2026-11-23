using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.MqttMessageHandlers
{
    [TopicHandler("urmet/cancelled")]
    public class UrmetCancelledCallHandler : IMqttMessageHandler
    {
        private readonly IBot _bot;
        private readonly IDatabase _db;

        public UrmetCancelledCallHandler(IBot bot, IDatabase database)
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
                    await _bot.SendMessage("📟 Nie zdążyłeś odebrać!");
                }

                await _db.WorkAsync(async (conn) =>
                {
                    using var fetchCmd = conn.CreateCommand();
                    fetchCmd.CommandText = "SELECT timestamp FROM calls WHERE number = $number ORDER BY timestamp DESC LIMIT 1;";//"INSERT INTO calls VALUES ($number, $timestamp, 'calling', 0)";
                    fetchCmd.Parameters.AddWithValue("$number", number).DbType = System.Data.DbType.Int16;
                    using var reader = await fetchCmd.ExecuteReaderAsync();

                    var insertNewRow = async () =>
                    {
                        using var insertCmd = conn.CreateCommand();
                        insertCmd.CommandText = "INSERT INTO calls VALUES ($number, $timestamp, 'cancelled', 0)";
                        insertCmd.Parameters.AddWithValue("$number", number).DbType = System.Data.DbType.Int16;
                        insertCmd.Parameters.AddWithValue("$timestamp", timestamp).DbType = System.Data.DbType.Int64;
                        await insertCmd.ExecuteNonQueryAsync();
                    };

                    if (!reader.HasRows)
                    {
                        await insertNewRow();
                    }
                    else
                    {
                        await reader.ReadAsync();
                        var callTimestamp = reader.GetInt64(0);
                        if (timestamp - callTimestamp > 120)
                        {
                            await insertNewRow();
                        }
                        else
                        {
                            using var updateCmd = conn.CreateCommand();
                            updateCmd.CommandText = "UPDATE calls SET state = 'cancelled' WHERE number = $number AND timestamp = $timestamp;";
                            updateCmd.Parameters.AddWithValue("$number", number).DbType = System.Data.DbType.Int16;
                            updateCmd.Parameters.AddWithValue("$timestamp", timestamp).DbType = System.Data.DbType.Int64;
                            await updateCmd.ExecuteNonQueryAsync();
                        }
                    }
                });
            }
        }
    }
}