using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageQuizBot.Models
{
    public class UserData
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime? LastQuizDate { get; set; }

        public int? SelectedLexiconId { get; set; }

        [ForeignKey(nameof(SelectedLexiconId))]
        public Lexicon? SelectedLexicon { get; set; }
    }
}
