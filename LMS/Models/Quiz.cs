using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }

        public string QuizName { get; set; }

        [ForeignKey("CursoId")]
        public virtual Curso Curso { get; set; }

        [Display(Name = "Curso")]
        public int CursoId { get; set; }

   
        public int QuestionQuantity { get; set; }


        public int maximunscore { get; set; }


    }
}
