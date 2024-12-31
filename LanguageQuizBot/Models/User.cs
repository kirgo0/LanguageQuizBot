using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageQuizBot.Models
{
    public class User
    {
        public string Id { get; set; }

        public long TelegramId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsAuthorized { get; set; } = false;

        public User(long telegramId, string userName)
        {
            TelegramId = telegramId;
            UserName = userName;
        }

        public User() { }
    }
}
