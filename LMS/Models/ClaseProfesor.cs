using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class ClaseProfesor
    {

        public int Id { get; set; }

        public int ProfesorId { get; set; }

        public Profesores Profesores { get; set; }

        public int ClaseId { get; set; }

        public Clase Clases { get; set; }

        [Display(Name = "Nombre")]
        public string NombreClase { get; set; }

     
    }
}
