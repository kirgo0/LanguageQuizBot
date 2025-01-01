using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageQuizBot.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public ICollection<UserWordGrade> UserWordGrades { get; set; }
        public ICollection<Translation> Translations { get; set; }
        public ICollection<LexiconWord> LexiconWords { get; set; }
    }
}
