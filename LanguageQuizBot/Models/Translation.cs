using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageQuizBot.Models
{
    public class Translation
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Value { get; set; }

        [MaxLength(255)]
        public string? Note { get; set; }

        public int WordId { get; set; }

        [ForeignKey(nameof(WordId))]
        public Word Word { get; set; }

        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }
    }
}
