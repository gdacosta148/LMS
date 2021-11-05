using LMS.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class UsuarioyClaseViewModel
    {
        public IEnumerable<ApplicationUser> UsuariosList { get; set; }

        // public IEnumerable<Clase> ClaseList { get; set; }


        //public IEnumerable<Materia> MateriaList { get; set; }
        public Profesores Profesores { get; set; }

        public string StatusMessage { get; set; }

        public List <CheckBoxItem>  ClasesDisponibles { get; set; }

        public List<CheckBoxItem> MateriasDisponibles { get; set; }



    }
}
