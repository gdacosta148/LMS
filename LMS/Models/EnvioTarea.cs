using LMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class EnvioTarea /*: IValidatableObject*/
    {
        public int Id { get; set; }

        //public int TareaId { get; set; }

        //public Tareas Tarea { get; set; }

        public  int CalificacionId { get; set; }

        [ForeignKey("CalificacionId")]
        public virtual Calificaciones Calificacion { get; set; }



        [AllowHtml]
        public string Descripcion { get; set; }


        //[Display(Name = "Uploaded File")]
        //public String FileName { get; set; }
        //public byte[] FileContent { get; set; }


        [NotMapped]

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Please select a file.")]

        //agregar docx y xlsx, ya que ya funciono lo de la extension pdf

        [MaxFileSize(40 * 1024 * 1024)]
        //[FileExtensions(ErrorMessage = "La extension del archivo no es valida", Extensions = ".pdf")]

        [AllowedExtensions(new string[] { ".pdf", ".docx",".xlsx "})]
        public IFormFile File { get; set; }

        //[Required]
        //[FileExtensions(ErrorMessage = "La extension del archivo no es valida", Extensions = "pdf")]
        public string FileName => File?.FileName;
        public string FileURL { get; set; }


        //public int CalificacionTareaId { get; set; }

        //[ForeignKey("CalificacionTareaId")]

        //public CalificacionTarea CalificacionTarea { get; set; }




    }
}
