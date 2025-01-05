using LanguageQuizBot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LanguageQuizBot.Services
{
    public class UserService
    {
        private ILogger<UserService> _logger;
        private ApplicationDbContext _dbContext;

        public UserService(ApplicationDbContext dbContext, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<User?> GetUserByTelegramIdAsync(long id)
        {
            return await _dbContext.Users.Where(u => u.TelegramId == id).Include(u => u.Data).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateUserAsync(long telegramId, string userName)
        {
            var user = new User(telegramId, userName) { Data = new() };
            await _dbContext.AddAsync(user);
            return (await _dbContext.SaveChangesAsync()) > 0;
        }

        public async Task<bool> SelectLexicon(User user, int lexiconId)
        {
            if(user.Data is null)
            {
                throw new NullReferenceException("The User's Data property must not be null for the SelectLexicon operation to be performed");
            }

            if (user.Data.SelectedLexiconId.HasValue && user.Data.SelectedLexiconId == lexiconId)
            {
                return true;
            }

            var lexicon = _dbContext.Lexicons.Where(l => l.Id == lexiconId).FirstOrDefaultAsync();
            
            if (lexicon is null) {
                _logger.LogWarning("The specified lexiconId {lexiconId} was not found", lexiconId);
                return false;
            }

            user.Data.SelectedLexiconId = lexiconId;

            if (!_dbContext.Entry(user).IsKeySet)
            {
                _dbContext.Attach(user);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
