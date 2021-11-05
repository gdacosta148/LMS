 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class Materia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nombre Materia")]
        public string Nombre { get; set; }

        public List<MateriaProfesor> MateriaProfesor;
    }
}
