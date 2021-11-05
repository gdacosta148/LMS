using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMS.Models
{
    public class RadioButton
    {
        public int Id { get; set; }

        public bool IsChecked { get; set; }


        public int QuizQuestionId { get; set; }

        [AllowHtml]
        public string QuizOption { get; set; }
    }
}
