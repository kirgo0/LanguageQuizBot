
namespace LanguageQuizBot.Models
{
    public class Language
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public ICollection<Translation> Translations { get; set; }
    }
}
