using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class MateriaProfesor
    {
        public int Id { get; set; }

        public int ProfesorId { get; set; }

        public Profesores Profesores { get; set; }

        public int MateriaId { get; set; }

        public Materia Materias { get; set; }


        [Display(Name = "Nombre")]
        public string NombreMateria { get; set; }

        

    
    }
}
