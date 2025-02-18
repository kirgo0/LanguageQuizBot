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
    [HandlerMetadata(Metatags.HandleType, UpdateType.CallbackQuery)]
    public class CallbackUpdateHandler : UpdateHandler
    {
        public CallbackUpdateHandler(ITelegramBotClient bot, ILifetimeScope scope, ILogger<CallbackUpdateHandler> logger) : base(bot, scope, logger)
        {
        }

        public override async Task Handle(Update update)
        {
            if (GetAction(update).Equals("_")) return;
            await RedirectHandle(
                update,
                Metatags.HandleAction,
                (value) => update.CallbackQuery.Data.StartsWith(value.ToString()),
                "An error occurred while resolving the action handler for {data}",
                update?.CallbackQuery?.Data
                );
        }
    }

}
