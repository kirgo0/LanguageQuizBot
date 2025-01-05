using LanguageQuizBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LanguageQuizBot.Extensions
{
    public static class TelegramBotClientExtensions
    {
        public static async Task<Message> BuildAndSendAsync(this ITelegramBotClient bot, MessageBuilder builder)
        {
            return await bot.SendMessage(
                builder.ChatId,
                builder.Text,
                parseMode: builder.ParseMode,
                replyMarkup: builder.ButtonsMarkup
                );
        }

        public static async Task<Message> BuildAndEditAsync(this ITelegramBotClient bot, MessageBuilder builder)
        {
            if (builder.LastMessageId == 0) throw new ArgumentException("The last message Id parameter is missing", nameof(builder.LastMessageId));

            return await bot.EditMessageText(
                builder.ChatId,
                builder.LastMessageId,
                builder.Text,
                parseMode: builder.ParseMode,
                replyMarkup: builder.ButtonsMarkup
                );
        }
    }
}
