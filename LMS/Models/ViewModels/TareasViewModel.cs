using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class TareasViewModel
    {


        public IEnumerable<Clase> ClaseList { get; set; }

        public IEnumerable<Materia> MateriaList { get; set; }

        public IEnumerable<Curso> CursoList { get; set; }


        //public List<CheckBoxItem> ClaseTarea { get; set; }

        //public List<CheckBoxItem> TareaMaterias { get; set; }

        public Curso Curso { get; set; }

        public Tareas Tareas { get; set; }

        public string StatusMessage { get; set; }

        public List<Tareas> TareasList { get; set; }

        public string dateaux { get; set; }

    }
}
