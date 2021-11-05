using LMS.Data;
using LMS.Extensions;
using LMS.Models;
using LMS.Models.ViewModels;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Areas.Usuarios.Controllers
{
    [Authorize(Roles = SD.TeacherUser)]
    [Area("Usuarios")]
    public class TareasController : Controller
    {
        private readonly ApplicationDbContext _db;

        private IWebHostEnvironment Environment;
        public IEnumerable<Estudiantes> EstudiantesList { get; set; }

        [BindProperty]
        public TareasViewModel TareasModel { get; set; }
        [TempData]
        public string StatusMessage { get; set; }

        private readonly IAuthorizationService _authorizationService;

        public TareasController(ApplicationDbContext db, IWebHostEnvironment _environment, IAuthorizationService authorizationService)
        {
            _db = db;

            Environment = _environment;

            _authorizationService = authorizationService;

            TareasModel = new TareasViewModel()
            {
                Tareas = new Models.Tareas()



            };
        }
        public async Task<IActionResult> Index()
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var Tareas = await _db.Tareas.Include(p => p.Prof)./*Include(p => p.Clase)*/Include(p => p.Materia).Include(c => c.Cursos)
                .Where(u => u.Prof.UserId == claim.Value).ToListAsync();

            return View(Tareas);
        }


        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> TareasCursoIndex(int? id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

         

            TareasModel.TareasList = await _db.Tareas.Include(p => p.Prof).Include(p => p.Materia).Include(c => c.Cursos)
                .Where(u => u.Prof.UserId == claim.Value && u.CursoId == id).ToListAsync();

            TareasModel.Tareas = await _db.Tareas.Include(p => p.Prof).Include(p => p.Materia).Include(c => c.Cursos)
                .Where(u => u.Prof.UserId == claim.Value && u.CursoId == id).FirstAsync();

            TareasModel.Curso = await _db.Curso.FindAsync(id);

            return View(TareasModel);
        }



        [Authorize(Roles = SD.TeacherUser)]
        public async Task<IActionResult> Create()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            //var item = _db.Clase.ToList();

            //var mat = _db.Materia.ToList();


            //var materia = await _db.MateriaProfesors.Where(m => m.ProfesorId == Prof.Id).FirstOrDefaultAsync();

            //var clase = await _db.ClaseProfesors.Where(c => c.ProfesorId == Prof.Id).FirstOrDefaultAsync();

            //toca buscar la manera de conectar materia profesor con materias

            TareasViewModel model = new TareasViewModel()
            {
                



                Tareas = new Models.Tareas(),


                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.Where(c => c.Profesor.UserId == Prof.UserId).ToListAsync()
            };



            model.Tareas.UserId = Prof.Id;

            //model.ClaseTarea = item.Select(vm => new CheckBoxItem()
            //{
            //    Id = vm.Id,
            //    Title = vm.Nombre,
            //    IsChecked = false

            //}).ToList();

            //model.TareaMaterias = mat.Select(m => new CheckBoxItem()

            //{
            //    Id = m.Id,
            //    Title = m.Nombre,

            //    IsChecked = false
            //}).ToList();
          

            return View(model);
        }

        //Create-Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(TareasViewModel model)
        {
            //List<ClaseTarea> stc = new List<ClaseTarea>();

            //List<TareaMateria> mtc = new List<TareaMateria>();

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            CursoClase Cursod = await _db.CursoClases.Where(p => p.CursoId == model.Tareas.CursoId).SingleAsync();

            ICollection<Estudiantes> Est = await _db.Estudiantes.Include(p => p.Clase).Where(p => p.ClaseId == Cursod.ClaseId).ToListAsync();



            Calificaciones calificacion;

            if (ModelState.IsValid)
            {
                var doesTareaExist = _db.Tareas.Include(p => p.Prof).Include(p => p.Cursos).Where(p => p.NombreTarea == model.Tareas.NombreTarea && p.CursoId == model.Tareas.CursoId && p.Id == model.Tareas.Id);

                if (doesTareaExist.Count() > 0)
                {
                    StatusMessage = "Error: Esa Tarea ya existe bajo el nombre de:" + doesTareaExist.First().NombreTarea + ",por favor, use otro nombre";
                }
                else
                {
                   

                    model.Tareas.UserId = Prof.Id;

                   
                    _db.Tareas.Add(model.Tareas);

                    await _db.SaveChangesAsync();

                    //for (int i = 0; i < Est.Count; i++)
                    //{

                    //    var stud = Est.Select(p => p.Id).ElementAtOrDefault(i);
                    //    calificacion = new Calificaciones();

                    //    calificacion.EstudianteId = stud;

                    //    //desmarcar de ser necesari, me refiero a calificacion.tareaid

                    //    calificacion.TareaId = model.Tareas.Id;

                    //    //calificacion.Date = model.Calificaciones.Date;

                    //    //calificacion.EndDate = model.Calificaciones.EndDate;


                    //    calificacion.Nombre = model.Tareas.NombreTarea;
                    //    calificacion.Nota = "Sin definir";

                    //    _db.Calificaciones.Add(calificacion);
                    //    //calificacion.EstudianteId = Est.
                    //}

                    //await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            TareasViewModel modelVM = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),
           

                Tareas = model.Tareas,



                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),
                CursoList = await _db.Curso.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }


        //Edit-GET
        [Authorize(Roles = SD.TeacherUser)]
        public async Task<IActionResult> Edit(int? id)
        {  


          

            if (id == null)
            {
                return NotFound();
            }

            var Tareas = await _db.Tareas.Include(t => t.Prof).Where(m => m.Id == id).SingleOrDefaultAsync();

            if (Tareas == null)
            {
                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Tareas, "TareaAuthorization");

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




            TareasViewModel model = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),

                

                Tareas = Tareas,

                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.ToListAsync()
            };

            string d = Tareas.Date.ToString("0:yyyy-MM-ddTHH:mm");

            model.dateaux = d;
            return View(model);
        }

        //Edit-Post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, TareasViewModel model)
        {

            if (ModelState.IsValid)
            {
                //var doesTareaExist = _db.Tareas.Include(p => p.Prof).Include(p => p.Materia).Where(p => p.Prof.Id == model.Tareas.UserId && p.NombreTarea == model.Tareas.NombreTarea);

                //if (doesTareaExist.Count() > 0)
                //{
                //    StatusMessage = "Error: Esa Actividad ya existe bajo el nombre de:" + doesTareaExist.First().NombreTarea + "por favor, use otro nombre";
                //}
                //else
                //{
                    var TareasFromDB = await _db.Tareas.FindAsync(id);

                    TareasFromDB.NombreTarea = model.Tareas.NombreTarea;

                TareasFromDB.Material = model.Tareas.Material;

                TareasFromDB.MateriaId = model.Tareas.MateriaId;
                    TareasFromDB.CursoId = model.Tareas.CursoId;

                TareasFromDB.Date = model.Tareas.Date;

                TareasFromDB.EndDate = model.Tareas.EndDate;

                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                //}
            }

            var Tareas = await _db.Tareas.Include(t => t.Prof).Where(m => m.Id == id).SingleOrDefaultAsync();
            TareasViewModel modelVM = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Tareas = Tareas,

                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }

        //GET Delete
        [Authorize(Roles = SD.TeacherUser)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Tarea = await _db.Tareas.Include(p => p.Prof).Include(c => c.Cursos).Include(p => p.Materia).Where(m => m.Id == id).SingleOrDefaultAsync();
            if (Tarea == null)
            {
                return NotFound();
            }


            var result = await _authorizationService.AuthorizeAsync(User, Tarea, "TareaAuthorization");

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

            return View(Tarea);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _db.Tareas.Include(t => t.Prof).SingleOrDefaultAsync(m => m.Id == id);

            //var calificacion = await _db.Calificaciones.FirstOrDefaultAsync(m => m.TareaId == tarea.Id);


            //La Icolection de calificaciones servia para borrar la calificacion en cuanto se borrara la tarea, pero esta ahora
            //se eliminara en cuanto el respectivo modelo de modelo enviotarea sea eliminado

            //ICollection<Calificaciones> calificacion = await _db.Calificaciones.Include(c => c.Estudiante).Include(c => c.Tareas).
            //    Where(c => c.TareaId == tarea.Id).ToListAsync();




            //if (calificacion != null)
            //{
            //    for (int i = 0; i < calificacion.Count; i++)
            //    {
            //        var calificacionid = calificacion.Select(p => p.Id).ElementAtOrDefault(i);

            //        //Calificaciones Cal1 = new Calificaciones();

            //        var cal2 = await _db.Calificaciones.Include(c => c.Tareas).Where(t => t.TareaId == id).Where(m => m.Id == calificacionid).SingleOrDefaultAsync();



            //        //Cal1.Id = calificacionid;

            //        _db.Calificaciones.Remove(cal2);


                  
            //    }

            //    await _db.SaveChangesAsync();


            //}

            _db.Tareas.Remove(tarea);

            await _db.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public JsonResult UploadFile(IList<IFormFile> uploadFiles)
        {
            string returnImagePath = string.Empty;
            string filename, extension, imageName, imageSavePath;

            //string wwwPath = this.Environment.WebRootPath;
            //string contentPath = this.Environment.ContentRootPath;

            //string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //List<string> uploadedFiles = new List<string>();

            foreach (IFormFile file in uploadFiles)
            {

                //string fileName = Path.GetFileName(postedFile.FileName);
                //using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                //{
                //    postedFile.CopyTo(stream);
                //    uploadedFiles.Add(fileName);
                //    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                //}
            if (file.Length > 0)
            {
                filename = Path.GetFileNameWithoutExtension(file.FileName);
                extension = Path.GetExtension(file.FileName);
                imageName = filename + DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
                imageSavePath = MyServer.MapPath("~/images/") + imageName + extension;
                file.SaveAs(imageSavePath);
                returnImagePath = "~/images/" + imageName + extension;
            }
        }
            return Json(Convert.ToString(returnImagePath), System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }


        public async Task<IActionResult> CrearTarea(int? id)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();


            if(id == null)
            {

                return NotFound();
            }

            var Tarea = await _db.Tareas.Include(p => p.Prof).Include(c => c.Cursos).Include(p => p.Materia).Where(m => m.CursoId == id).FirstOrDefaultAsync();
            if (Tarea == null)
            {
                return NotFound();
            }

            var result = await _authorizationService.AuthorizeAsync(User, Tarea, "TareaAuthorization");

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

            TareasViewModel model = new TareasViewModel()
            {




                Tareas = new Models.Tareas(),


                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.Where(c => c.Id == id).ToListAsync(),

                Curso = await _db.Curso.Where(c => c.Id == id).SingleOrDefaultAsync()
            };



            model.Tareas.UserId = Prof.Id;




            return View(model);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CrearTarea(TareasViewModel model)
        {


            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Profesores Prof = await _db.Profesores.Where(p => p.UserId == claim.Value).FirstOrDefaultAsync();

            CursoClase Cursod = await _db.CursoClases.Where(p => p.CursoId == model.Tareas.CursoId).SingleAsync();

            ICollection<Estudiantes> Est = await _db.Estudiantes.Include(p => p.Clase).Where(p => p.ClaseId == Cursod.ClaseId).ToListAsync();



            Calificaciones calificacion;

            if (ModelState.IsValid)
            {
                var doesTareaExist = _db.Tareas.Include(p => p.Prof).Include(p => p.Cursos).Where(p => p.NombreTarea == model.Tareas.NombreTarea && p.CursoId == model.Tareas.CursoId);

                if (doesTareaExist.Count() > 0)
                {
                    StatusMessage = "Error: Esa Tarea ya existe bajo el nombre de:" + doesTareaExist.First().NombreTarea + ",por favor, use otro nombre";
                }
                else
                {


                    model.Tareas.UserId = Prof.Id;
                    _db.Tareas.Add(model.Tareas);


                    await _db.SaveChangesAsync();

                    //for (int i = 0; i < Est.Count; i++)
                    //{

                    //    var stud = Est.Select(p => p.Id).ElementAtOrDefault(i);
                    //    calificacion = new Calificaciones();

                    //    calificacion.EstudianteId = stud;

               

                    //    calificacion.TareaId = model.Tareas.Id;

              


                    //    calificacion.Nombre = model.Tareas.NombreTarea;
                    //    calificacion.Nota = "Sin definir";

                    //    _db.Calificaciones.Add(calificacion);
       
                    //}



                    //await _db.SaveChangesAsync();

                    //return RedirectToAction(nameof(Index));

                    return RedirectToAction("TareasCursoIndex", "Tareas", new { id = model.Tareas.CursoId });
                }
            }

            TareasViewModel modelVM = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),


                Tareas = model.Tareas,


                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.Where(c => c.Id == model.Curso.Id).ToListAsync(),

                Curso = await _db.Curso.Where(c => c.Id == model.Curso.Id).SingleOrDefaultAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }


        [Authorize(Roles = SD.TeacherUser)]
        public async Task<IActionResult> CursoEdit(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

            var Tareas = await _db.Tareas.Include(p => p.Prof).Where(m => m.Id == id).SingleOrDefaultAsync();

            if (Tareas == null)
            {
                return NotFound();
            }


            //var Tarea = await _db.Tareas.Include(p => p.Prof).Include(c => c.Cursos).Include(p => p.Materia).Where(m => m.CursoId == id).SingleOrDefaultAsync();
        

            var result = await _authorizationService.AuthorizeAsync(User, Tareas, "TareaAuthorization");

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


            TareasViewModel model = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Tareas = Tareas,

                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.Where(t => t.Id == Tareas.CursoId).ToListAsync(),

                Curso = await _db.Curso.Where(c => c.Id == Tareas.CursoId).SingleOrDefaultAsync(),
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CursoEdit(int id, TareasViewModel model)
        {

            if (ModelState.IsValid)
            {
                //var doesTareaExist = _db.Tareas.Include(p => p.Prof).Include(p => p.Materia).Where(p => p.Prof.Id == model.Tareas.UserId && p.NombreTarea == model.Tareas.NombreTarea);

                //if (doesTareaExist.Count() > 0)
                //{
                //    StatusMessage = "Error: Esa Actividad ya existe bajo el nombre de:" + doesTareaExist.First().NombreTarea + "por favor, use otro nombre";
                //}
                //else
                //{
                var TareasFromDB = await _db.Tareas.FindAsync(id);

                TareasFromDB.NombreTarea = model.Tareas.NombreTarea;

                TareasFromDB.Material = model.Tareas.Material;

                TareasFromDB.MateriaId = model.Tareas.MateriaId;
                TareasFromDB.CursoId = model.Tareas.CursoId;

                await _db.SaveChangesAsync();

                return RedirectToAction("TareasCursoIndex", "Tareas", new { id = model.Tareas.CursoId });
                //}
            }

            var Tareas = await _db.Tareas.Include(t => t.Prof).Where(m => m.Id == id).SingleOrDefaultAsync();
            TareasViewModel modelVM = new TareasViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),



                Tareas = Tareas,

                MateriaList = await _db.Materia.ToListAsync(),

                ClaseList = await _db.Clase.ToListAsync(),

                CursoList = await _db.Curso.Where(c => c.Id == model.Curso.Id).ToListAsync(),

                Curso = await _db.Curso.Where(c => c.Id == model.Curso.Id).SingleOrDefaultAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }


        //GET Delete
        [Authorize(Roles = SD.TeacherUser)]
        public async Task<IActionResult> CursoDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Tarea = await _db.Tareas.Include(p => p.Prof).Include(c => c.Cursos).Include(p => p.Materia).Where(m => m.Id == id).SingleOrDefaultAsync();
            if (Tarea == null)
            {
                return NotFound();
            }


            var result = await _authorizationService.AuthorizeAsync(User, Tarea, "TareaAuthorization");

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

            return View(Tarea);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> CursoDelete(int id)
        {
            var tarea = await _db.Tareas.SingleOrDefaultAsync(m => m.Id == id);

            _db.Tareas.Remove(tarea);

            await _db.SaveChangesAsync();

            return RedirectToAction("TareasCursoIndex", "Tareas", new { id = tarea.CursoId });

        }





    }
}