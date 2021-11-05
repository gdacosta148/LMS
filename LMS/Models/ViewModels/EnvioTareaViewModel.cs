using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class EnvioTareaViewModel
    {

        public EnvioTarea EnvioTarea { get; set; }


        public IEnumerable<EnvioTarea> EnvioTareaList { get; set; }

        public DateTime LocalDate;

        public bool test;
        public IEnumerable<Calificaciones> Calificaciones {get; set;}

    
        public Calificaciones Calificacionesid { get; set; }
        public string StatusMessage { get; set; }
    }
}
