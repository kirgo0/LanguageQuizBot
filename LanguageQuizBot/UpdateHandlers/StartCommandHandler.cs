using Autofac;
using LanguageQuizBot.Attributes;
using LanguageQuizBot.Common;
using LanguageQuizBot.UpdateHandlers.Abstractions;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LanguageQuizBot.UpdateHandlers
{
    [HandleCommand(Commands.START_COMMAND)]
    public class StartCommandHandler : UpdateHandler
    {
        public StartCommandHandler(ITelegramBotClient bot, ILifetimeScope scope, ILogger<MessageUpdateHandler> logger) : base(bot, scope, logger)
        {
        }

        public override async Task Handle(Update update)
        {
            //return base.Handle(update);
        }
    }
}
