using LMS.Utility;
using Microsoft.AspNetCore.Http;
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
    public class Calificaciones
    {
        [Key]
        public int Id { get; set; }



        [Display(Name = "Calificación")]
        public string Nota { get; set; }






        [Display(Name = "Tarea")]
        public int TareaId { get; set; }



        [ForeignKey("TareaId")]
        public virtual Tareas Tareas { get; set; }

        //public IList <CalificaciónClase> CalificaciónClases { get; set; }


        public int EstudianteId { get; set; }



        [Display(Name = "Quiz")]
        public int? QuizId { get; set; }

        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }

        [ForeignKey("EstudianteId")]
        public virtual Estudiantes Estudiante { get; set; }

        public String Nombre { get; set; }


        public int  Enviado { get; set; }


        [Display(Name = "No Enviado")]
        public int  Noenviado { get; set; }


        public int Calificado { get; set; }

        [Display(Name = "No Calificado")]
        public int NoCalificado { get; set;  }


        //[Display(Name = "Plazo de inicio:")]
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        //public DateTime Date { get; set; }

        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]

        //[Display(Name = "Plazo de entrega:")]
        //public DateTime EndDate { get; set; }

        //public int EnvioTareaId { get; set; }

        //[ForeignKey("EnvioTareaId")]
        //public EnvioTarea EnvioTarea { get; set; }

        [AllowHtml]
        public string ComentarioProfesor { get; set; }

        [NotMapped]

        [DataType(DataType.Upload)]
  

        //agregar docx y xlsx, ya que ya funciono lo de la extension pdf

        [MaxFileSize(40 * 1024 * 1024)]
        //[FileExtensions(ErrorMessage = "La extension del archivo no es valida", Extensions = ".pdf")]

        [AllowedExtensions(new string[] { ".pdf", ".docx", ".xlsx " })]
        public IFormFile RevisionTarea { get; set; }

        //[Required]
        //[FileExtensions(ErrorMessage = "La extension del archivo no es valida", Extensions = "pdf")]
        public string FileName => RevisionTarea?.FileName;
        public string FileURL { get; set; }

    }
}
