
namespace LanguageQuizBot.Models
{
    public class Lexicon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<LexiconWord> LexiconWords { get; set; }
    }
}
