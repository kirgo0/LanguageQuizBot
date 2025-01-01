using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageQuizBot.Models
{
    public class Translation
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int WordId { get; set; }

        [ForeignKey(nameof(WordId))]
        public Word Word { get; set; }

        public int LanguageId { get; set; }

        [ForeignKey(nameof(LanguageId))]
        public Language Language { get; set; }
    }
}
