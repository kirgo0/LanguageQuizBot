﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageQuizBot.Models
{
    public class UserWordGrade
    {
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int WordId { get; set; }

        [ForeignKey(nameof(WordId))]
        public Word Word { get; set; }

        public int Grade { get; set; }
    }
}