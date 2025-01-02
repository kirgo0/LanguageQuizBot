
using System.ComponentModel.DataAnnotations;

namespace LanguageQuizBot.Models
{
    public class Language
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Value { get; set; }

        public ICollection<Translation> Translations { get; set; }
    }
}
