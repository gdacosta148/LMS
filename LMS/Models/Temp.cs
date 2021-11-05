using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class Temp
    {
        [Key]
        [Display(Name = "Respuesta")]
        public int Id { get; set; }

    
        [AllowHtml]
        public string Option { get; set; }


      


    }
}
