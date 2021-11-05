using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Clase
    {
        [Key]
        public int Id { get; set; }

        [Required]

        [Display(Name = "Nombre Clase")]
        public string Nombre { get; set; }

        public List<ClaseProfesor> ClaseProfesor { get; set; }

        public List<CursoClase> CursoClase { get; set; }
         
    }
}
