using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class CalificacionyTareaViewModel
    {
        //public IEnumerable<Tareas> TareasList { get; set; }



        public Calificaciones Calificaciones { get; set; }

        //public IEnumerable<Estudiantes> EstudiantesList { get; set; }

        public Curso Curso { get; set; }
        public string StatusMessage { get; set; }

        public IEnumerable<Tareas> TareasCollection { get; set; }

        public IEnumerable<Estudiantes> EstudiantesCollection { get; set; }

        public EnvioTarea EnvioTarea { get; set; }

       public  Tareas Tarea { get; set; }
        //public Clase Clase { get; set; }

    }
}
