using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageQuizBot.Models
{
    public class User
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }

        [MaxLength(32)]
        public string Name { get; set; } = string.Empty;

        public int DataId { get; set; }
        [ForeignKey(nameof(DataId))]
        public UserData Data { get; set; }

        public User(long telegramId, string userName)
        {
            TelegramId = telegramId;
            Name = userName;
        }

        public User() { }
    }
}
