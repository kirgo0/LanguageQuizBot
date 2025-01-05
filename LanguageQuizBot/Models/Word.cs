using System.ComponentModel.DataAnnotations;

namespace LanguageQuizBot.Models
{
    public class Word
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string Value { get; set; }
        public ICollection<UserWordGrade> UserWordGrades { get; set; }
        public ICollection<Translation> Translations { get; set; }
        public ICollection<LexiconWord> LexiconWords { get; set; }
    }
}
