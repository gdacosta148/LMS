using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class Tareas
    {
        [Key]
        public int Id { get; set; }



        [Required]
        [Display(Name = "Actividad")]
        public string NombreTarea { get; set; }

        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        [ForeignKey("MateriaId")]
        public virtual Materia Materia { get; set; }

        //[Display(Name = "Clase")]
        //public int ClaseId { get; set; }

        //[ForeignKey("ClaseId")]
        //public virtual Clase Clase { get; set; }

        [Display(Name = "Profesor")]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual Profesores Prof { get; set; }


        //public List<ClaseTarea> ClaseTarea { get; set; }

        //public List<TareaMateria> TareaMaterias { get; set; }

        [Display(Name = "Curso")]
        public int CursoId { get; set; }

        [ForeignKey("CursoId")]
        public virtual Curso Cursos { get; set; }

        //Materialapoyo tareas

        [AllowHtml]
        public string Material { get; set; }


        [Display(Name = "Plazo de inicio:")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]

        [Display(Name = "Plazo de entrega:")]
        public DateTime EndDate { get; set; }


  

    }
}
