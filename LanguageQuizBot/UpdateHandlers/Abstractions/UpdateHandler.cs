using Autofac.Features.Metadata;
using Autofac;
using System.Globalization;
using Telegram.Bot;
using Microsoft.Extensions.Logging;
using LanguageQuizBot.Repository.Interfaces;
using LanguageQuizBot.Models;
using Telegram.Bot.Types;
using User = LanguageQuizBot.Models.User;

namespace LanguageQuizBot.UpdateHandlers.Abstractions
{
    public abstract class UpdateHandler
    {
        protected readonly CultureInfo _culture = new CultureInfo("uk-UA");

        protected readonly ITelegramBotClient _bot;

        protected readonly string botName = Environment.GetEnvironmentVariable("BOT_NAME");

        protected readonly ILifetimeScope _scope;

        protected readonly ILogger _log;

        protected IUserRepository _userRepository;

        private Task<User> _userTask;

        protected Task<User> userTask
        {
            get
            {
                if (!NeedsUser)
                {
                    _log.LogWarning("Be careful, you are requesting the user without specifying the {name} attribute inside the {handler} constructor", nameof(NeedsUser), GetType().Name);
                    return Task.FromResult<User>(null);
                }
                return _userTask;
            }
            set
            {
                _userTask = value;
            }
        }
        public bool NeedsUser { get; set; } = false;

        private Task<Chat> _chatTask;

        protected UpdateHandler(ITelegramBotClient bot, ILifetimeScope scope, ILogger logger)
        {
            _bot = bot;
            _scope = scope;
            _log = logger;
        }

        public abstract Task Handle(Update update);

        protected async Task PrepareAndHandle(
            Update update,
            Task<User> userTask = null
            )
        {
            if (NeedsUser && userTask is null)
            {
                _userRepository = _scope.Resolve<IUserRepository>();
                if (userTask is null) _userTask = TryGetOrCreateUser(update);
                else _userTask = userTask;
            }

            await Handle(update);
        }

        public virtual async Task RedirectHandle(
            Update update,
            string serviceMetaTag,
            Predicate<object> comparator,
            string resolvingErrorMessage,
            params object[] resolveErrorParams
            )
        {
            UpdateHandler handler = null;
            try
            {
                object value;
                var handlers = _scope.Resolve<IEnumerable<Meta<UpdateHandler>>>();
                foreach (var item in handlers)
                {
                    if (!item.Metadata.TryGetValue(serviceMetaTag, out value))
                        continue;
                    if (comparator(value))
                    {
                        handler = item.Value;
                        break;
                    }
                    if (handler is not null) break;
                }
                if (handler is null)
                {
                    _log.LogDebug("No handler found for {params}", resolveErrorParams);
                    return;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, resolvingErrorMessage, resolveErrorParams);
                return;
            }
            try
            {
                await handler.PrepareAndHandle(update, _userTask);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An error occurred while handling a request with the {handler}", handler);
            }
        }

        protected string GetAction(Update update)
        {
            if (update?.CallbackQuery?.Data is null)
            {
                _log.LogWarning("Update has no callback data in {type}", GetType());
                return string.Empty;
            }
            return update.CallbackQuery.Data;
        }

        protected IEnumerable<string> GetParams(Update update)
        {
            try
            {
                var parts = update?.Message?.Text?.Split(' ');
                return parts?.Skip(1) ?? new List<string>();
            }
            catch (Exception ex)
            {
                _log.LogWarning(ex, "An error occured while getting params for update {update}", update.ToString());
                return new List<string>();
            }
        }

        protected async Task<User> TryGetOrCreateUser(Update update)
        {
            Telegram.Bot.Types.User from = null;
            if (update?.Message?.From is not null)
                from = update.Message.From;
            else if (update?.CallbackQuery?.From is not null)
                from = update.CallbackQuery.From;

            if (from is null) throw new NullReferenceException("The sender of the request was unable to be identified");

            var user = await _userRepository.GetByTelegramIdAsync(from.Id);

            if (user is null)
            {
                user = await _userRepository.CreateAsync(new User(from.Id, $"{from.FirstName}{from.LastName}"));
            }
            return user;
        }
    }
}
