using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class CursoViewModel
    {



        //el profesro se asigna con el id del usuario actual o profesor actual

        public Curso Curso { get; set; }

    
        public IEnumerable<CursoClase> CursoClaseList { get; set; }

        public IEnumerable<Calificaciones> CalificacionesList{ get; set; }
        public IEnumerable<Tareas> TareasList { get; set; }
       
        public List<CheckBoxItem> ClasesDisponibles { get; set; }

  
        Curso Cursos { get; set; }
        public Calificaciones Calificacion { get; set; }


        public IEnumerable<Estudiantes> EstudiantesCollection { get; set; }
        //agregar variable clase en el modelo del curso para poder filtrar el contenido en el index,para que el estudiante solo vea los cursos que esten
        //asociados a el


        public IEnumerable<Quiz> QuizList { get; set; }

        public Quiz Quiz { get; set; }

        public IEnumerable<QuizQuestion> QuizQuestions { get; set; }


        public QuizQuestion QuizQuestion { get; set; }
        public string StatusMessage { get; set; }

    }
}
