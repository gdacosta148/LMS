using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class QuizQuestion
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        [Display(Name = "Quiz")]
        public int QuizId { get; set; }


        [Display(Name = "Question name")]
        public string Question { get; set; }

        [AllowHtml]
        public string QuizOption1 { get; set; }

        [AllowHtml]
        public string QuizOption2{ get; set; }

        [AllowHtml]
        public string QuizOption3 { get; set; }

        [AllowHtml]
        public string QuizOption4 { get; set; }


        public string correctanswer { get; set; }


 



    }
}
