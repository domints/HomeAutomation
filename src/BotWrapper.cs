using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using src.Config;
using Telegram.Bot;

namespace src
{
    public interface IBot
    {
        Task SendMessage(string text);
    }
    
    public class BotWrapper : IBot
    {
        private readonly TelegramBotClient _bot;
        private readonly string _chatId;

        public BotWrapper(TelegramBotClient botClient, IOptions<TelegramConfig> config)
        {
            _bot = botClient;
            _chatId = config.Value.ChatId!;
        }

        public Task SendMessage(string text)
        {
            return _bot.SendMessage(_chatId, text);
        }
    }
}