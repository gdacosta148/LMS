using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Models.ViewModels
{
    public class QuizViewModel
    {
        public IEnumerable<Quiz> QuizList { get; set; }

        public Quiz Quiz { get; set; }

        public IEnumerable<QuizQuestion> QuizQuestions { get; set; }


        public QuizQuestion QuizQuestion { get; set; }


        public List <QuizOptions> QuizOptionsList { get; set; }

        public IEnumerable<Temp> AnswerCollection{ get; set; }

        public Temp Answer { get; set; }

    }
}
