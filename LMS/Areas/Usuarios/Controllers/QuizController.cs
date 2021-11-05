using LMS.Data;
using LMS.Extensions;
using LMS.Models;
using LMS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    [Authorize(Roles = "Profesor,Estudiante")]
    public class QuizController : Controller
    {
        // GET: QuizController

        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public QuizViewModel QuizView { get; set; }

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IAuthorizationService _authorizationService;

        public QuizController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IAuthorizationService authorizationService)
        {
            _db = db;

            _authorizationService = authorizationService;

            QuizView = new QuizViewModel()
            {
                Quiz = new Quiz(),

                QuizQuestion = new QuizQuestion(),

               Answer = new Temp()


            };

            _webHostEnvironment = webHostEnvironment;
        }




        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> QuestionIndex(int? id)
        {


            QuizView.Quiz = await _db.Quiz.Include(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(m => m.Id == id).SingleOrDefaultAsync();

            QuizView.QuizQuestions = await _db.QuizQuestion.Where(m => m.QuizId == id).ToListAsync();

            var result = await _authorizationService.AuthorizeAsync(User, QuizView.Quiz, "QuizAuthorization");




            if (!result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }


            return View(QuizView);
        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CreateQuestion(int? id)
        {

            QuizView.Quiz = await _db.Quiz.Include(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(m => m.Id == id).SingleOrDefaultAsync();

            QuizView.QuizQuestions = await _db.QuizQuestion.Where(m => m.QuizId == id).ToListAsync();




  

        

            var result = await _authorizationService.AuthorizeAsync(User, QuizView.Quiz, "QuizAuthorization");

            if (!result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }

            return View(QuizView);
        }


        //Create-Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CreateQuestion(int id)
        {


            if (ModelState.IsValid)
            {
                var doesQuiz = _db.QuizQuestion.Include(p => p.Quiz).Where(p => p.Question == QuizView.QuizQuestion.Question);

                if (doesQuiz.Count() > 0)
                {
                    StatusMessage = "Error: Ese Quiz ya existe bajo el nombre de:" + doesQuiz.First().Question + ",por favor, use otro nombre";
                }
                else
                {







                    QuizView.QuizQuestion.QuizId = id;


                    //model.Curso.ProfesorId = Prof.Id;
                    _db.QuizQuestion.Add(QuizView.QuizQuestion);

                    await _db.SaveChangesAsync();





                    return RedirectToAction("CreateQuestionAnswer", "Quiz", new { id = id});



                }




            }






            QuizViewModel modelVM = new QuizViewModel()
            {

                Quiz = QuizView.Quiz,

                QuizQuestion = new Models.QuizQuestion()
            
            };




            return View(modelVM);
        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CreateQuestionAnswer(int? id , int? QuestionId)
        {

            QuizView.Quiz = await _db.Quiz.Include(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(m => m.Id == id).SingleOrDefaultAsync();

            QuizView.QuizQuestion = await _db.QuizQuestion.Include(p => p.Quiz).Where(m => m.Id == QuestionId).SingleOrDefaultAsync();


           QuizView.AnswerCollection = await _db.Temp.ToListAsync();


         

            var result = await _authorizationService.AuthorizeAsync(User, QuizView.Quiz, "QuizAuthorization");

            if (!result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }

            return View(QuizView);
        }


        //Create-Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CreateQuestionAnswer(int id )
        {

            int a = QuizView.Answer.Id;

            if (ModelState.IsValid)
            {





                var QQuestion = await _db.QuizQuestion.Include(q => q.Quiz).ThenInclude(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(c => c.QuizId == id).SingleOrDefaultAsync();

            var QuizQuestion = await _db.QuizQuestion.FindAsync(QQuestion.Id);

              

                    if (QuizView.Answer.Id == 1)
                    {

                        QuizQuestion.correctanswer = QuizView.QuizQuestion.QuizOption1;
                    }else if (QuizView.Answer.Id == 2)
                    {
                        QuizQuestion.correctanswer = QuizView.QuizQuestion.QuizOption2;

                    }
                    else if (QuizView.Answer.Id == 3)
                    {
                        QuizQuestion.correctanswer = QuizView.QuizQuestion.QuizOption3;
                    }
                    else if (QuizView.Answer.Id == 4)
                    {
                        QuizQuestion.correctanswer = QuizView.QuizQuestion.QuizOption4;

                    }



               

                    QuizQuestion.QuizOption1 = QuizView.QuizQuestion.QuizOption1;

                    QuizQuestion.QuizOption2 = QuizView.QuizQuestion.QuizOption2;

                    QuizQuestion.QuizOption3 = QuizView.QuizQuestion.QuizOption3;

                    QuizQuestion.QuizOption4 = QuizView.QuizQuestion.QuizOption4;

                    //QuizQuestion.correctanswer = QuizView.QuizQuestion.correctanswer;

                    await _db.SaveChangesAsync();





                    return RedirectToAction("QuestionIndex", "Quiz", new { id = id });



                




            }






            QuizViewModel modelVM = new QuizViewModel()
            {

                Quiz = QuizView.Quiz,

                QuizQuestion = new Models.QuizQuestion()

            };




            return View(modelVM);
        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> EditQuestion(int? id)
        {

          

            QuizView.QuizQuestions = await _db.QuizQuestion.Where(m => m.QuizId == id).ToListAsync();

            QuizView.QuizQuestion = await _db.QuizQuestion.Where(m => m.QuizId == id).SingleOrDefaultAsync();

            QuizView.Quiz = await _db.Quiz.Include(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(m => m.Id == QuizView.QuizQuestion.Id).SingleOrDefaultAsync();



            var result = await _authorizationService.AuthorizeAsync(User, QuizView.Quiz, "QuizAuthorization");

            if (!result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }

            return View(QuizView);
        }


        //Edit-Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> EditQuestion(int id)
        {


            if (ModelState.IsValid)
            {
                var doesQuiz = _db.QuizQuestion.Include(p => p.Quiz).Where(p => p.Question == QuizView.QuizQuestion.Question);

                if (doesQuiz.Count() > 0)
                {
                    StatusMessage = "Error: Ese Quiz ya existe bajo el nombre de:" + doesQuiz.First().Question + ",por favor, use otro nombre";
                }
                else
                {


                    var QuizQuestion = await _db.QuizQuestion.Include(q => q.Quiz).ThenInclude(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(c => c.Id == id).SingleOrDefaultAsync();


                    QuizQuestion.Id = QuizView.QuizQuestion.QuizId;

                    QuizQuestion.correctanswer = QuizView.QuizQuestion.correctanswer;

           
    
                    QuizQuestion.QuizOption1 = QuizView.QuizQuestion.QuizOption1;

                    QuizQuestion.QuizOption2 = QuizView.QuizQuestion.QuizOption2;

                    QuizQuestion.QuizOption3 = QuizView.QuizQuestion.QuizOption3;

                    QuizQuestion.QuizOption4 = QuizView.QuizQuestion.QuizOption4;

                    QuizQuestion.correctanswer = QuizView.QuizQuestion.correctanswer;
                    //model.Curso.ProfesorId = Prof.Id;
                 

              

                    await _db.SaveChangesAsync();






                    await _db.SaveChangesAsync();
                    return RedirectToAction("QuizIndex", "Curso", new { id = id });



                }




            }






            QuizViewModel modelVM = new QuizViewModel()
            {

                Quiz = new Models.Quiz(),

                QuizQuestion = new Models.QuizQuestion()

            };




            return View(modelVM);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> DeleteQuestion(int? id)
        {

            QuizView.Quiz = await _db.Quiz.Include(p => p.Curso).ThenInclude(q => q.Profesor).ThenInclude(q => q.ApplicationUser).Where(m => m.Id == id).SingleOrDefaultAsync();

            QuizView.QuizQuestions = await _db.QuizQuestion.Where(m => m.QuizId == id).ToListAsync();



            var result = await _authorizationService.AuthorizeAsync(User, QuizView.Quiz, "QuizAuthorization");

            if (!result.Succeeded)
            {
                if (User.Identity.IsAuthenticated)
                {
                    return new ForbidResult();
                }
                else
                {
                    return new ChallengeResult();
                }
            }

            return View(QuizView);
        }


        //POST Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var Question = await _db.QuizQuestion.SingleOrDefaultAsync(m => m.Id == id);



            _db.QuizQuestion.Remove(Question);

      

            await _db.SaveChangesAsync();


            return RedirectToAction("QuizIndex", "Curso", new { id = id });
        }



    }
}
