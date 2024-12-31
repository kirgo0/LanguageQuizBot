using Autofac;
using LanguageQuizBot.UpdateHandlers.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot;
using LanguageQuizBot.Helpers;
using Telegram.Bot.Types.Enums;

namespace LanguageQuizBot.UpdateHandlers
{
    public class DefaultUpdateHandler : UpdateHandler
    {
        public DefaultUpdateHandler(ITelegramBotClient bot, ILifetimeScope scope, ILogger<DefaultUpdateHandler> logger) : base(bot, scope, logger)
        {
        }

        public override async Task Handle(Update update)
        {
            await RedirectHandle(
                update,
                Metatags.HandleType,
                (type) => (UpdateType)type == update.Type,
                "An error occured while resolving update handler for type {type}",
                update.Type
            );
        }
    }

}
