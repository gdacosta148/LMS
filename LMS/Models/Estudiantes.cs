using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Estudiantes
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }


        [Display(Name = "Nombre")]
        public string NombreEstudiante { get; set; }

        [ForeignKey("UserId")]

        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Clase")]
        public int ClaseId { get; set; }

        [ForeignKey("ClaseId")]

        public virtual Clase Clase { get; set; }


    }
}
