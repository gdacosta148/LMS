using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Curso
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "NombreCurso")]
        public string Nombre{ get; set; }

       

        [Display(Name = "Profesor")]
        public int ProfesorId { get; set; }


        [ForeignKey("ProfesorId")]
        public Profesores Profesor { get; set; }

        public List<CursoClase> CursoClases{ get; set; }

        
   

    }
}
