using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class EstudianteyClaseViewModel
    {
        public IEnumerable<ApplicationUser> UsuariosList { get; set; }

        public IEnumerable<Clase> ClaseList { get; set; }


      
        public Estudiantes Estudiantes { get; set; }

        public string StatusMessage { get; set; }



    }
}
