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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace LMS.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]

    public class CalificacionesController : Controller
    {
        private readonly ApplicationDbContext _db;


        private readonly IWebHostEnvironment _webHostEnvironment;
        [TempData]
        public string StatusMessage { get; set; }

        private readonly IAuthorizationService _authorizationService;
        public CalificacionesController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IAuthorizationService authorizationService)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;

            _authorizationService = authorizationService;

        }

        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Curso Curso = await _db.Curso.Where(p => p.Profesor.UserId == claim.Value).FirstOrDefaultAsync();

            var Calificaciones = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(c => c.Cursos)
                .Include(p => p.Estudiante).Include(p => p.Tareas.Materia).
              Where(u => u.Estudiante.UserId == claim.Value || u.Tareas.Prof.UserId == claim.Value).ToListAsync();



            return View(Calificaciones);
        }






        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Create()
        {


            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();



            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //TareasList = await _db.Tareas.Where(u => u.Prof.UserId == claim.Value).ToListAsync(),

                Calificaciones = new Models.Calificaciones(),



                TareasCollection = await _db.Tareas.Where(u => u.Prof.UserId == claim.Value).ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes/*.Where(c => c.ClaseId == Prof.ClaseId)*/.ToListAsync()

                
                //EstudiantesList = await _db.Estudiantes.Where(c => c.ClaseId == Prof.ClaseId).ToListAsync()




            };



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(CalificacionyTareaViewModel model)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            //Tareas tareas = await _db.Tareas.Where(p => p.Prof.UserId == claim.Value).FirstOrDefaultAsync();

            //revisar que la query cursoc funcione, es la mas recinetemente creada ya que se creo el 1 de junio de 2021 

            //CursoClase Cursod = await _db.CursoClases.Where(p => p.CursoId == model.Calificaciones.Tareas.CursoId).SingleAsync();

            CursoClase cursoc = await _db.CursoClases.Where(p => p.Curso.Profesor.UserId == claim.Value).FirstOrDefaultAsync();

            ICollection<Estudiantes> Est = await _db.Estudiantes.Include(p => p.Clase).Where(p => p.ClaseId == /*tareas.ClaseId*/ cursoc.ClaseId)./*.Where(p => p.ClaseId == Prof.ClaseId)*/ToListAsync();

            // toca remodelar todo por la clase, ya que un profesor puede tener más de una clase

            Calificaciones calificacion;



            if (ModelState.IsValid)
            {
                var doesCalificacionExist = _db.Calificaciones/*.Include(p => p.Tareas)*/.Include(p => p.Estudiante).Where(p => p.Estudiante.Id == model.Calificaciones.EstudianteId);

                if (doesCalificacionExist.Count() > 0)
                {
                    StatusMessage = "Error: Esa Calificacion ya existe bajo el nombre de:" + doesCalificacionExist.First().Id + ",por favor, use otro nota";
                }
                else
                {
                  
                    //Crear modelo estudiantesy asignarlo al id del profesor

                    //Crear modelo calificaciones, para crear uno nuevo cada vez que pase el bucle

                    for (int i = 0; i < Est.Count; i++)
                    {

                        var stud = Est.Select(p => p.Id).ElementAtOrDefault(i);
                        calificacion = new Calificaciones();

                        calificacion.EstudianteId = stud;

                        //desmarcar de ser necesari, me refiero a calificacion.tareaid

                        calificacion.TareaId = model.Calificaciones.TareaId;

                        //calificacion.Date = model.Calificaciones.Date;

                        //calificacion.EndDate = model.Calificaciones.EndDate;

                        Tareas t = await _db.Tareas.Where(t => t.Id == calificacion.TareaId).SingleOrDefaultAsync();

                        calificacion.Nombre = t.NombreTarea;
                        calificacion.Nota = "Sin definir";

                        _db.Calificaciones.Add(calificacion);
                        //calificacion.EstudianteId = Est.
                    }
                    // _db.Calificaciones.Add(model.Calificaciones);



                    await _db.SaveChangesAsync();







                    return RedirectToAction(nameof(Index));
                }
            }

            CalificacionyTareaViewModel modelVM = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),


                Calificaciones = new Models.Calificaciones(),

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Edit(int? id)
        {
     

            if (id == null )
            {
                return NotFound();
            }

            var Calificaciones = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(t => t.Prof).SingleOrDefaultAsync(m => m.Id == id);

            if (Calificaciones == null)
            {
                return NotFound();
            }


            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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


            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Calificaciones = Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, CalificacionyTareaViewModel model)
        {

            if (ModelState.IsValid)


            {
                var doesCalicacionExist = _db.Calificaciones.Include(p => p.Tareas).Include(p => p.Estudiante).Where(p => p.Estudiante.Id == model.Calificaciones.EstudianteId);

                var CalificacionesFromDB = await _db.Calificaciones.FindAsync(id);

                //CalificacionesFromDB.Date = model.Calificaciones.Date;

                //CalificacionesFromDB.EndDate = model.Calificaciones.EndDate;

                CalificacionesFromDB.Nota = model.Calificaciones.Nota;







                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            CalificacionyTareaViewModel modelVM = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Calificaciones = model.Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }


        //GET Delete

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            var Calificaciones = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(p => p.Prof).Include(p => p.Estudiante).SingleOrDefaultAsync(m => m.Id == id);
            if (Calificaciones == null)
            {
                return NotFound();
            }


            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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

            return View(Calificaciones);
        }


        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var calificacion = await _db.Calificaciones.SingleOrDefaultAsync(m => m.Id == id);
            _db.Calificaciones.Remove(calificacion);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CrearCalificacion(int? id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            if (id == null)
            {
                return NotFound();
            }


         


            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //TareasList = await _db.Tareas.Where(u => u.Prof.UserId == claim.Value).ToListAsync(),

                Calificaciones = new Models.Calificaciones(),



                TareasCollection = await _db.Tareas.Where(u => u.Prof.UserId == claim.Value && u.CursoId == id ).ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes/*.Where(c => c.ClaseId == Prof.ClaseId)*/.ToListAsync(),

                Curso = await _db.Curso.FindAsync(id)
            //EstudiantesList = await _db.Estudiantes.Where(c => c.ClaseId == Prof.ClaseId).ToListAsync()




        };



            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CrearCalificacion(CalificacionyTareaViewModel model)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            CursoClase Cursod = await _db.CursoClases.Where(p => p.CursoId == model.Curso.Id).SingleAsync();

            //CursoClase cursoc = await _db.CursoClases.Where(p => p.Curso.Profesor.UserId == claim.Value).FirstOrDefaultAsync();

           

            ICollection<Estudiantes> Est = await _db.Estudiantes.Include(p => p.Clase).Where(p => p.ClaseId ==  Cursod.ClaseId).ToListAsync();



            Calificaciones calificacion;


   
         
          

            if (ModelState.IsValid)
            {
                var doesCalificacionExist = _db.Calificaciones.Include(p => p.Tareas).Include(p => p.Estudiante).Where(p =>  p.Tareas.Id == model.Calificaciones.TareaId);

                
                if (doesCalificacionExist.Count() > 0)
                {
                    StatusMessage = "Error: Esa Calificacion ya existe bajo el nombre de:" + doesCalificacionExist.First().Tareas.Id + ",por favor, use otro nombre";
                }
                else
                {


                    for (int i = 0; i < Est.Count; i++)
                    {

                        var stud = Est.Select(p => p.Id).ElementAtOrDefault(i);
                        calificacion = new Calificaciones();

                        calificacion.EstudianteId = stud;

                        //desmarcar de ser necesari, me refiero a calificacion.tareaid

                        calificacion.TareaId = model.Calificaciones.TareaId;

                        //calificacion.Date = model.Calificaciones.Date;

                        //calificacion.EndDate = model.Calificaciones.EndDate;

                        Tareas t = await _db.Tareas.Where(t => t.Id == calificacion.TareaId).SingleOrDefaultAsync();

                        calificacion.Nombre = t.NombreTarea;
                        calificacion.Nota = "Sin definir";

                        _db.Calificaciones.Add(calificacion);
                        //calificacion.EstudianteId = Est.
                    }
                    // _db.Calificaciones.Add(model.Calificaciones);


                    await _db.SaveChangesAsync();

                  

               




                    return RedirectToAction("Calificaciones", "Calificaciones", new { id = model.Curso.Id });
                }
            }

        

        CalificacionyTareaViewModel modelVM = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),


                Calificaciones = new Models.Calificaciones(),

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);



        }


        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CalificarTarea(int? id)
        {

            var doesEnvioexist = _db.EnvioTarea.Include(e => e.Calificacion).Where(p => p.Id == id);

            if (id == null || doesEnvioexist.Count() <= 0)
            {
                return NotFound();
            }


            var EnvioTarea = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();

            var Calificaciones = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Prof).Where(m => m.Id == EnvioTarea.CalificacionId).SingleOrDefaultAsync();

            if (Calificaciones == null)
            {
                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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



            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Calificaciones = Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync()
            };




            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CalificarTarea(int id, CalificacionyTareaViewModel model)
        {


            

            if (!ModelState.IsValid)
            {

                StatusMessage = "Error: La extension del archivo no es valida, solo se pueden subir archivos pdf,docx o xlsx:";
                return View(model);


            }

           


                string webroothpath = _webHostEnvironment.WebRootPath;

            if(model.Calificaciones.RevisionTarea != null) { 
                if (model.Calificaciones.RevisionTarea.Length > 0 )
                {
                    model.Calificaciones.FileURL = Guid.NewGuid() + Path.GetExtension(model.Calificaciones.RevisionTarea.FileName);

                    string fileurl = Path.GetFullPath(Path.Combine(webroothpath, "Files"));




                    using (var filestream = new FileStream(Path.Combine(fileurl, model.Calificaciones.FileURL), FileMode.Create))
                    {
                        await model.Calificaciones.RevisionTarea.CopyToAsync(filestream);

                    }



                }

            }




            //var doesCalicacionExist = _db.Calificaciones.Include(p => p.Tareas).Include(p => p.Estudiante).Where(p => p.Estudiante.Id == model.Calificaciones.EstudianteId);



                var CalificacionesFromDB = await _db.Calificaciones.FindAsync(id);

                //CalificacionesFromDB.Date = model.Calificaciones.Date;

                //CalificacionesFromDB.EndDate = model.Calificaciones.EndDate;

                CalificacionesFromDB.Nota = model.Calificaciones.Nota;

                CalificacionesFromDB.ComentarioProfesor = model.Calificaciones.ComentarioProfesor;


                CalificacionesFromDB.FileURL = model.Calificaciones.FileURL;

                 CalificacionesFromDB.Calificado = 1;

            CalificacionesFromDB.NoCalificado = 0;

                     


                await _db.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));


            return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = id });
        }



        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> Calificaciones(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            Curso Curso = await _db.Curso.Where(p => p.Profesor.UserId == claim.Value).FirstOrDefaultAsync();

            var Calificaciones = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(c => c.Cursos)
                .Include(p => p.Estudiante).Include(p => p.Tareas.Materia).
              Where(u => (u.Estudiante.UserId == claim.Value || u.Tareas.Prof.UserId == claim.Value) && u.Tareas.CursoId == id).ToListAsync();


       



            return View(Calificaciones);
        }


        //EditarCalificaicon sirve para editar la nota o el archivo revisado que envia el profesor

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> EditarCalificacion(int? id)
        {


            var doesEnvioexist = _db.EnvioTarea.Include(e => e.Calificacion).Where(p => p.Id == id);
            if (id == null || doesEnvioexist.Count() <= 0)
            {
                return NotFound();
            }


            var EnvioTarea = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();

            var Calificaciones = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(t => t.Prof).Where(m => m.Id == EnvioTarea.CalificacionId).SingleOrDefaultAsync();

            if (Calificaciones == null)
            {
                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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



            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Calificaciones = Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync()
            };


            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EditarCalificacion(int id, CalificacionyTareaViewModel model)
        {

            Calificaciones Cal = await _db.Calificaciones.Where(c => c.Id == id).SingleOrDefaultAsync();

            string url = Cal.FileURL;

            if (!ModelState.IsValid)
            {

                StatusMessage = "Error: La extension del archivo no es valida, solo se pueden subir archivos pdf,docx o xlsx:";
                return View(model);


            }



            string webroothpath = _webHostEnvironment.WebRootPath;

            string uploadsFolder = Path.Combine(webroothpath, "Files");

            if (url != null)
            {
                var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, url);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }


            if (model.Calificaciones.RevisionTarea != null)
            {
                if (model.Calificaciones.RevisionTarea.Length > 0)
                {
                    model.Calificaciones.FileURL = Guid.NewGuid() + Path.GetExtension(model.Calificaciones.RevisionTarea.FileName);

                    string fileurl = Path.GetFullPath(Path.Combine(webroothpath, "Files"));




                    using (var filestream = new FileStream(Path.Combine(fileurl, model.Calificaciones.FileURL), FileMode.Create))
                    {
                        await model.Calificaciones.RevisionTarea.CopyToAsync(filestream);

                    }



                }

            }


            var doesCalicacionExist = _db.Calificaciones.Include(p => p.Tareas).Include(p => p.Estudiante).Where(p => p.Estudiante.Id == model.Calificaciones.EstudianteId);



            var CalificacionesFromDB = await _db.Calificaciones.FindAsync(id);

            //CalificacionesFromDB.Date = model.Calificaciones.Date;

            //CalificacionesFromDB.EndDate = model.Calificaciones.EndDate;

            CalificacionesFromDB.Nota = model.Calificaciones.Nota;

            CalificacionesFromDB.ComentarioProfesor = model.Calificaciones.ComentarioProfesor;


            CalificacionesFromDB.FileURL = model.Calificaciones.FileURL;

         




            await _db.SaveChangesAsync();

            //return RedirectToAction(nameof(Index));

            return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = id });



        }


        //Se editan los valores como la fecha, la tarea,etc.
        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> EditarCalificacionCurso(int? id)
        {

            //var doesEnvioexist = _db.EnvioTarea.Include(e => e.Calificacion).Where(p => p.Id == id);

            if (id == null )
            {
                return NotFound();
            }

            var Calificaciones = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(t => t.Prof).Where(m => m.Id == id).FirstOrDefaultAsync();

            if (Calificaciones == null)
            {
                return NotFound();
            }


            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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


            CalificacionyTareaViewModel model = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Calificaciones = Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarCalificacionCurso(int id, CalificacionyTareaViewModel model)
        {

            if (ModelState.IsValid)


            {
                var doesCalicacionExist = _db.Calificaciones.Include(p => p.Tareas).Include(p => p.Estudiante).Where(p => p.Estudiante.Id == model.Calificaciones.EstudianteId);

                //var CalificacionesFromDB = await _db.Calificaciones.FindAsync(id);

                var CalificacionesFromDB = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(p => p.Cursos).Where(p => p.Id == id).SingleAsync();

                //CalificacionesFromDB.Date = model.Calificaciones.Date;

                //CalificacionesFromDB.EndDate = model.Calificaciones.EndDate;

                CalificacionesFromDB.Nota = model.Calificaciones.Nota;




             


                await _db.SaveChangesAsync();

                return RedirectToAction("Calificaciones", "Calificaciones", new { id = CalificacionesFromDB.Tareas.CursoId });

            }

            CalificacionyTareaViewModel modelVM = new CalificacionyTareaViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Calificaciones = model.Calificaciones,

                TareasCollection = await _db.Tareas.ToListAsync(),

                EstudiantesCollection = await _db.Estudiantes.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }




        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> CursoDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Calificaciones = await _db.Calificaciones.Include(p => p.Tareas).ThenInclude(t => t.Prof).Include(p => p.Estudiante).Where(m => m.Id == id).SingleOrDefaultAsync();
            if (Calificaciones == null)
            {
                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Calificaciones, "CalificarTareaAutorization");

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

            return View(Calificaciones);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CursoDelete(int id)
        {
            var calificacion = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(t => t.Cursos).Where(m => m.Id == id).SingleOrDefaultAsync();
            _db.Calificaciones.Remove(calificacion);
            await _db.SaveChangesAsync();
            return RedirectToAction("Calificaciones", "Calificaciones", new { id = calificacion.Tareas.CursoId });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveCalificacion(CursoViewModel c)
        {
            // add semester data

            return Json(c);
        }


    }
}