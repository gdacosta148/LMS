using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class QuizOptions
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("QuizQuestionId")]
        public virtual QuizQuestion QuizQuestion { get; set; }

        [Display(Name = "QuizQuestion")]
        public int QuizQuestionId { get; set; }

        [AllowHtml]
        public string QuizOption { get; set; }


       


    }
}
