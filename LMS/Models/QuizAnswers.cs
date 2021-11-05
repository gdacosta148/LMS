using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class QuizAnswers
    {
        [Key]
        public int Id { get; set;}


        [Display(Name = "QuizOption ")]
        public int QuizOptionId { get; set; }


        [ForeignKey("QuizOptionId")]
        public virtual QuizOptions QuizOption { get; set; }

        public string Answer { get; set; }
    }
}
