using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class CursoClase
    {
        public int Id { get; set; }

        public int ClaseId { get; set; }

        public Clase Clase { get; set; }

        public int CursoId { get; set; }

        public Curso Curso { get; set; }

        public string NombreClase { get; set; }

     

       
    }
}
