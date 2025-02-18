﻿using Autofac;
using LanguageQuizBot.Attributes;
using LanguageQuizBot.Helpers;
using LanguageQuizBot.UpdateHandlers.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LanguageQuizBot.UpdateHandlers
{
    [HandlerMetadata(Metatags.HandleType, UpdateType.Message)]
    public class MessageUpdateHandler : UpdateHandler
    {
        public MessageUpdateHandler(ITelegramBotClient bot, ILifetimeScope scope, ILogger<MessageUpdateHandler> logger) : base(bot, scope, logger)
        {
        }

        public override async Task Handle(Update update)
        {
            var sufix = $"@{botName}";

            if (update?.Message?.Text is null && update?.Message is not null)
            {
                if (update.Message.Sticker is not null) return;
                if (update.Message.Animation is not null) return;
                await RedirectHandle(
                    update,
                    Metatags.HandleMessageEvent,
                    (value) =>
                    {
                        var property = update.Message.GetType().GetProperty(value.ToString());
                        if (property is not null)
                            return property.GetValue(update.Message) is not null;
                        return false;
                    },
                    "An error occured while resolving message event handler for {update}",
                    update
                );
            }
            else
            {
                var command = update.Message.Text;
                if (!command.StartsWith("/")) return;
                if (command.Contains("@") && !command.Contains(sufix)) return;
                await RedirectHandle(
                    update,
                    Metatags.HandleCommand,
                    (value) =>
                    command.Split(' ')[0]
                        .Replace(sufix, "")
                        .Equals(value.ToString()),
                    "An error occurred while resolving the command handler for {text}",
                    update.Message.Text
                );
            }
        }
    }
}
