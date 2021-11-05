using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LMS.Data;
using LMS.Extensions;
using LMS.Models;
using LMS.Models.ViewModels;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]
    [Authorize(Roles = "Profesor,Estudiante")]

    public class CursoController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public CursoViewModel CursoView { get; set; }

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IAuthorizationService _authorizationService;
        public CursoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IAuthorizationService authorizationService)
        {
            _db = db;

            _authorizationService = authorizationService;

            CursoView = new CursoViewModel()
            {
                Curso = new Curso(),

                StatusMessage = StatusMessage
            };

            _webHostEnvironment = webHostEnvironment;


        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            Estudiantes est = await _db.Estudiantes.Include(p => p.Clase).Where(e => e.UserId == claim.Value).FirstOrDefaultAsync();

            //var CursoC = this.User.IsInRole("Profesor") ? await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
            //    .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(p => p.Profesor.UserId == claim.Value).ToListAsync() :
            //   await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser).
            //   Include(c => c.CursoClases.Where(p => p.ClaseId == est.ClaseId))
            //   .ToListAsync();



            var Curso2 = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(p => p.Profesor.UserId == claim.Value).ToListAsync();


            //Crear 2 index uno para profesores y otro para estudiantes , en el de estudiantes usar _db.cursoclases

            //var Cursos = await (from curso in _db.Curso
            //                join cursoclase in _db.CursoClases
            //                 on curso.Id equals cursoclase.CursoId
            //                join estudiante in _db.Estudiantes
            //                on cursoclase.ClaseId equals estudiante.ClaseId
            //                where cursoclase.ClaseId == estudiante.ClaseId
            //                || curso.Profesor.Id == Prof.Id
            //                select curso).ToListAsync();



            return View(Curso2);

        }

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> IndexEstudiantes()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);



            Estudiantes est = await _db.Estudiantes.Include(p => p.Clase).Where(e => e.UserId == claim.Value).FirstOrDefaultAsync();





            var Curso2 = await _db.CursoClases.Include(p => p.Curso).ThenInclude(p => p.Profesor).ThenInclude(p => p.ApplicationUser).
                Where(p => p.ClaseId == est.ClaseId).ToListAsync();


            //Crear 2 index uno para profesores y otro para estudiantes , en el de estudiantes usar _db.cursoclases


            return View(Curso2);

        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Create()
        {

            var Clase = _db.Clase.ToList();

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            //CursoViewModel model = new CursoViewModel()
            //{

            //    Curso = new Models.Curso(),
            //    StatusMessage = StatusMessage
            //};

            //model.ClasesDisponibles = Clase.Select(vm => new CheckBoxItem()
            //{
            //    Id = vm.Id,
            //    Title = vm.Nombre,
            //    IsChecked = false

            //}).ToList();


     
        
            CursoView.ClasesDisponibles = Clase.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false
            }).ToList();

            //model.Curso.ProfesorId = Prof.Id;


            return View(CursoView);
        }


        //Create-Post
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CreatePost(/*CursoViewModel model*/)
        {
            List<CursoClase> stc = new List<CursoClase>();
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var doesCursoExist = _db.Curso.Include(p => p.Profesor).Include(p => p.CursoClases).Where(p => p.Nombre == CursoView.Curso.Nombre);

                if (doesCursoExist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Curso ya existe bajo el nombre de:" + doesCursoExist.First().Nombre + ",por favor, use otro nombre";
                }
                else
                {

                   

                   
               

                    CursoView.Curso.ProfesorId = Prof.Id;

               
                    //model.Curso.ProfesorId = Prof.Id;
                    _db.Curso.Add(CursoView.Curso);

                    await _db.SaveChangesAsync();



                    foreach (var item in CursoView.ClasesDisponibles)
                    {
                        if (item.IsChecked  == true)
                        {
                            stc.Add(new CursoClase() { CursoId = CursoView.Curso.Id, ClaseId = item.Id });
                        }
                    }

            
                    foreach (var item in stc)
                    {
                        //StudentCourse StudentCourse = new StudentCourse();
                        var cn = _db.Clase.Where(c => c.Id == item.ClaseId).FirstOrDefault();
                        item.NombreClase = cn.Nombre;
                        _db.CursoClases.Add(item);

                         
                 /*       StudentCourse.CursoId = CursoView.Curso.Id*/;

                    }


             

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }




            }


            var Clases = _db.Clase.ToList();



            CursoViewModel modelVM = new CursoViewModel()
            {

                Curso = new Models.Curso(),
                StatusMessage = StatusMessage
            };

            modelVM.ClasesDisponibles = Clases.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList();

            modelVM.Curso.ProfesorId = Prof.Id;

            return View(modelVM);
        }


        //Edit-GET
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
            var Clases = _db.Clase.ToList();

            List<CursoClase> stc = new List<CursoClase>();
            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.ClasesDisponibles = Clases.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList();

            return View(CursoView);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            var Clases = _db.Clase.ToList();
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            List<CursoClase> stc = new List<CursoClase>();
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var Cursos = await _db.Curso.FindAsync(CursoView.Curso.Id);

                await _db.SaveChangesAsync();



                foreach (var item in CursoView.ClasesDisponibles)
                {
                    if (item.IsChecked == true)
                    {
                        stc.Add(new CursoClase() { CursoId = CursoView.Curso.Id, ClaseId = item.Id });
                    }
                }

                var tabledata = _db.CursoClases.Where(m => m.CursoId == id).ToList();

                var listresult = tabledata.Except(stc).ToList();

                foreach (var curso in listresult)
                {
                    _db.CursoClases.Remove(curso);
                    _db.SaveChanges();
                }

                var getcursoid = _db.CursoClases.Where(m => m.CursoId == id).ToList();

                foreach (var item in stc)
                {
                    if (!getcursoid.Contains(item))
                    {
                        var cn = _db.Clase.Where(c => c.Id == item.ClaseId).FirstOrDefault();
                        item.NombreClase = cn.Nombre;
                        _db.CursoClases.Add(item);

                        await _db.SaveChangesAsync();
                    }

                }

                Cursos.Nombre = CursoView.Curso.Nombre;




                await _db.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            CursoViewModel modelVM = new CursoViewModel()
            {

                Curso = new Models.Curso(),
                StatusMessage = StatusMessage
            };

            modelVM.ClasesDisponibles = Clases.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList();

            modelVM.Curso.ProfesorId = Prof.Id;

            return View(modelVM);


        }


        //GET : Delete MenuItem
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            if (CursoView.Curso == null)
            {
                return NotFound();
            }

            return View(CursoView);
        }

        //POST Delete MenuItem
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _db.Curso.SingleOrDefaultAsync(m => m.Id == id);

            _db.Curso.Remove(curso);
            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> CursoModule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(m => m.Id == id).SingleOrDefaultAsync();

            //var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();


            //Esta variable sirve para que la autorizacion del estudiante se detecte, en caso de que intente acceder a algun curso en el cual no este inscritoc.
            var CursoClases = await _db.CursoClases.Include(c => c.Curso).Include(c => c.Clase).Where(c => c.CursoId == id).SingleOrDefaultAsync();

            var Estudiantes = await _db.Estudiantes.Include(e => e.Clase).Include(e => e.ApplicationUser).Where(c => c.ClaseId == CursoClases.ClaseId).SingleOrDefaultAsync();


            //var Curso = await _db.Curso.Include(p => p.Profesor)/*ThenInclude(p => p.ApplicationUser)*/
            //    .Include(p => p.CursoClases)./*ThenInclude(p => p.Clase*//*)*/Where(m => m.Id == id && m.Profesor.UserId == Prof.UserId).SingleOrDefaultAsync();

            if (Estudiantes == null)
            {

                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Estudiantes, "EstudianteCursoAutorization");


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



            CursoView.Curso.Id = (int)id;

            if (CursoView.Curso == null)
            {
                return NotFound();
            }

   


            return View(CursoView);
        }




        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> ProfesorCursoModule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(m => m.Id == id).SingleOrDefaultAsync();


            CursoView.Curso.Id = (int)id;

            if (CursoView.Curso == null)
            {
                return NotFound();
            }

            var result2 = await _authorizationService.AuthorizeAsync(User, CursoView.Curso, "ProfesorCursoAuthorization");

            if (!result2.Succeeded)
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


            return View(CursoView);
        }




        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> Actividades(int? id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Calificaciones cal = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
               Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).FirstOrDefaultAsync();

            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(m => m.Id == id).SingleOrDefaultAsync();

            CursoView.TareasList = await _db.Tareas.Include(p => p.Cursos).ThenInclude(p => p.Profesor).Include(m => m.Materia).
                Where(t => t.CursoId == id).ToListAsync();

            CursoView.CalificacionesList = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).Include(p => p.Estudiante).
                  Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).ToListAsync();

            //CursoView.LocalDate = DateTime.Now;


            var CursoClases = await _db.CursoClases.Include(c => c.Curso).Include(c => c.Clase).Where(c => c.CursoId == id).SingleOrDefaultAsync();

            var Estudiantes = await _db.Estudiantes.Include(e => e.Clase).Include(e => e.ApplicationUser).Where(c => c.ClaseId == CursoClases.ClaseId).SingleOrDefaultAsync();


            if (Estudiantes == null)
            {

                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Estudiantes, "EstudianteCursoAutorization");


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



            if (CursoView.Curso == null)
            {
                return NotFound();
            }

            return View(CursoView);



        }






        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult>   Calificaciones(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Curso Curso = await _db.Curso.Where(p => p.Profesor.UserId == claim.Value).FirstOrDefaultAsync();

            var Calificaciones = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(c => c.Cursos)
                .Include(p => p.Estudiante).Include(p => p.Tareas.Materia).
              Where(u => (u.Estudiante.UserId == claim.Value || u.Tareas.Prof.UserId == claim.Value) && u.Tareas.CursoId == id).ToListAsync();


     
            CursoView.CalificacionesList = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(c => c.Cursos)
                .Include(p => p.Estudiante).Include(p => p.Tareas.Materia).
              Where(u => (u.Estudiante.UserId == claim.Value || u.Tareas.Prof.UserId == claim.Value) && u.Tareas.CursoId == id).ToListAsync();

            return View(CursoView);
        }


        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> CalificacionModule(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Calificaciones cal = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
               Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).FirstOrDefaultAsync();

            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.TareasList = await _db.Tareas.Include(p => p.Cursos).ThenInclude(p => p.Profesor).Include(m => m.Materia).
                Where(t => t.CursoId == id).ToListAsync();

            CursoView.CalificacionesList = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).Include(p => p.Estudiante).
                  Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).ToListAsync();

            //CursoView.LocalDate = DateTime.Now;



            if (CursoView.Curso == null)
            {
                return NotFound(CursoView);
            }


            return View(CursoView);
        }


        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CrearCalificacionesCurso(int? id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            CursoView.Calificacion = new Models.Calificaciones();

            CursoView.TareasList = await _db.Tareas.Where(u => u.Prof.UserId == claim.Value && u.Cursos.Id == id).ToListAsync();

            CursoView.EstudiantesCollection = await _db.Estudiantes/*.Where(c => c.ClaseId == Prof.ClaseId)*/.ToListAsync();

         
        

            return View(CursoView);
        }


        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> MaterialApoyo(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }



            var Tarea = await _db.Tareas.Where(t => t.Id == id).SingleOrDefaultAsync();




        

            return View(Tarea);



        }


        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> MaterialApoyoModule(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Calificaciones cal = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
               Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).FirstOrDefaultAsync();

            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.TareasList = await _db.Tareas.Include(p => p.Cursos).ThenInclude(p => p.Profesor).Include(m => m.Materia).
                Where(t => t.CursoId == id).ToListAsync();

            CursoView.CalificacionesList = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).Include(p => p.Estudiante).
                  Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).ToListAsync();

            //CursoView.LocalDate = DateTime.Now;



            if (CursoView.Curso == null)
            {
                return NotFound(CursoView);
            }


            return View(CursoView);
        }



        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> TareasModule(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Calificaciones cal = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
               Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).FirstOrDefaultAsync();

            if (id == null)
            {
                return NotFound();
            }

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
                .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.TareasList = await _db.Tareas.Include(p => p.Cursos).ThenInclude(p => p.Profesor).Include(m => m.Materia).
                Where(t => t.CursoId == id).ToListAsync();

            CursoView.CalificacionesList = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).Include(p => p.Estudiante).
                  Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value).ToListAsync();

            //CursoView.LocalDate = DateTime.Now;


            var CursoClases = await _db.CursoClases.Include(c => c.Curso).Include(c => c.Clase).Where(c => c.CursoId == id).SingleOrDefaultAsync();

            var Estudiantes = await _db.Estudiantes.Include(e => e.Clase).Include(e => e.ApplicationUser).Where(c => c.ClaseId == CursoClases.ClaseId).SingleOrDefaultAsync();


            if (Estudiantes == null)
            {

                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Estudiantes, "EstudianteCursoAutorization");


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



            if (CursoView.Curso == null)
            {
                return NotFound(CursoView);
            }


            return View(CursoView);
        }



        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> QuizModule(int? id)
        {
           



            if (id == null)
            {
                return NotFound();
            }





            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).SingleAsync();

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();

            var CursoClases = await _db.CursoClases.Include(c => c.Curso).Include(c => c.Clase).Where(c => c.CursoId == id).SingleOrDefaultAsync();

            var Estudiantes = await _db.Estudiantes.Include(e => e.Clase).Include(e => e.ApplicationUser).Where(c => c.ClaseId == CursoClases.ClaseId).SingleOrDefaultAsync();


            if (Estudiantes == null)
            {

                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Estudiantes, "EstudianteCursoAutorization");


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


            if (CursoView.Curso == null)
            {
                return NotFound(CursoView);
            }


            return View(CursoView);

        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> QuizIndex(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
              .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            return View(CursoView);
        }



        [Authorize(Roles = "Estudiante")]
        public async Task<IActionResult> PresentarQuiz(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
              .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            return View(CursoView);
        }


        
        //Create-Post
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> PresentarQuiz(int id)
        {
            List<CursoClase> stc = new List<CursoClase>();
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                var doesCursoExist = _db.Curso.Include(p => p.Profesor).Include(p => p.CursoClases).Where(p => p.Nombre == CursoView.Curso.Nombre);

                if (doesCursoExist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Curso ya existe bajo el nombre de:" + doesCursoExist.First().Nombre + ",por favor, use otro nombre";
                }
                else
                {






                    CursoView.Curso.ProfesorId = Prof.Id;


                    //model.Curso.ProfesorId = Prof.Id;
                    _db.Curso.Add(CursoView.Curso);

                    await _db.SaveChangesAsync();



                    foreach (var item in CursoView.ClasesDisponibles)
                    {
                        if (item.IsChecked == true)
                        {
                            stc.Add(new CursoClase() { CursoId = CursoView.Curso.Id, ClaseId = item.Id });
                        }
                    }


                    foreach (var item in stc)
                    {
                        //StudentCourse StudentCourse = new StudentCourse();
                        var cn = _db.Clase.Where(c => c.Id == item.ClaseId).FirstOrDefault();
                        item.NombreClase = cn.Nombre;
                        _db.CursoClases.Add(item);


                        /*       StudentCourse.CursoId = CursoView.Curso.Id*/
                        ;

                    }




                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }




            }


            var Clases = _db.Clase.ToList();



            CursoViewModel modelVM = new CursoViewModel()
            {

                Curso = new Models.Curso(),
                StatusMessage = StatusMessage
            };

            modelVM.ClasesDisponibles = Clases.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList();

            modelVM.Curso.ProfesorId = Prof.Id;

            return View(modelVM);
        }




        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CrearQuiz(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
    .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            var result2 = await _authorizationService.AuthorizeAsync(User, CursoView.Curso, "ProfesorCursoAuthorization");

            if (!result2.Succeeded)
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


            return View(CursoView);
        }


        //Create-Post
        [HttpPost, ActionName("CrearQuiz")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CrearQuiz(int id)
        {
         

            if (ModelState.IsValid)
            {
                var doesCursoExist = _db.Quiz.Include(p => p.Curso).Where(p => p.QuizName == CursoView.Quiz.QuizName);

                if (doesCursoExist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Quiz ya existe bajo el nombre de:" + doesCursoExist.First().QuizName + ",por favor, use otro nombre";
                }
                else
                {





          

                    CursoView.Quiz.CursoId = id;
           

                    //model.Curso.ProfesorId = Prof.Id;
                    _db.Quiz.Add(CursoView.Quiz);

                    await _db.SaveChangesAsync();






                    await _db.SaveChangesAsync();
                    return RedirectToAction("QuizIndex","Curso", new { id = id });

                  

                }




            }


  



            CursoViewModel modelVM = new CursoViewModel()
            {

                Curso = new Models.Curso(),
                StatusMessage = StatusMessage
            };

        

            modelVM.Curso.Id = id;

            return View(modelVM);
        }



        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> EditarQuiz(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
    .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            var result2 = await _authorizationService.AuthorizeAsync(User, CursoView.Curso, "ProfesorCursoAuthorization");

            if (!result2.Succeeded)
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


            return View(CursoView);
        }


        [HttpPost, ActionName("EditarQuiz")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarQuizPost(int id)
        {
           
       

            if (ModelState.IsValid)
            {
                var Quiz= await _db.Quiz.Include(c=> c.Curso).Where(c => c.CursoId == id).SingleOrDefaultAsync();

                



          


                Quiz.QuizName= CursoView.Quiz.QuizName;

                Quiz.QuestionQuantity = CursoView.Quiz.QuestionQuantity;

                Quiz.maximunscore = CursoView.Quiz.maximunscore;


                await _db.SaveChangesAsync();


                return RedirectToAction("QuizIndex", "Curso", new { id = id });
            }

            CursoViewModel modelVM = new CursoViewModel()
            {

                Curso = new Models.Curso(),
                Quiz = new Models.Quiz(),
                StatusMessage = StatusMessage
            };


            modelVM.Quiz.CursoId = id;
            modelVM.Curso.Id=  id;

            return View(modelVM);


        }





        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> DeleteQuiz(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
    .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            var result2 = await _authorizationService.AuthorizeAsync(User, CursoView.Curso, "ProfesorCursoAuthorization");

            if (!result2.Succeeded)
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


            return View(CursoView);
        }



        [HttpPost, ActionName("DeleteQuiz")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteQuizPost(int id)
        {
            var quiz = await _db.Quiz.Where(m => m.CursoId == id).SingleOrDefaultAsync();

            _db.Quiz.Remove(quiz);
            await _db.SaveChangesAsync();



            return RedirectToAction("QuizIndex", "Curso", new { id = id });
        }




        //[Authorize(Roles = "Profesor")]
        //public async Task<IActionResult> PreguntaIndex(int? id)
        //{

        //    CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
        //      .Include(p => p.CursoClases).ThenInclude(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);

        //    CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



        //    CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


        //    return View(CursoView);
        //}




        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CrearPregunta(int? id)
        {

            CursoView.Curso = await _db.Curso.Include(p => p.Profesor).ThenInclude(p => p.ApplicationUser)
    .Include(p => p.CursoClases).ThenInclude(p => p.Clase).Where(m => m.Id == id).SingleOrDefaultAsync();

            CursoView.QuizList = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).ToListAsync();



            CursoView.Quiz = await _db.Quiz.Include(C => C.Curso).Where(c => c.CursoId == id).FirstOrDefaultAsync();


            var result2 = await _authorizationService.AuthorizeAsync(User, CursoView.Curso, "ProfesorCursoAuthorization");

            if (!result2.Succeeded)
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


            return View(CursoView);
        }




    }
}
