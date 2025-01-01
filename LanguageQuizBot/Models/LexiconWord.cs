using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageQuizBot.Models
{
    public class LexiconWord
    {
        public int LexiconId { get; set; }

        [ForeignKey(nameof(LexiconId))]
        public Lexicon Lexicon { get; set; }

        public int WordId { get; set; }

        [ForeignKey(nameof(WordId))]
        public Word Word { get; set; }
    }
}
