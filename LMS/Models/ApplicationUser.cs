using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name = "Nombre de Usuario")]
        public string Nombre { get; set; }


        public string Telefono { get; set; }

        public string Ciudad { get; set; }






    }
}
