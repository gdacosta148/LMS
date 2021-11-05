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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nClam;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Areas.Usuarios.Controllers
{
    [Area("Usuarios")]

    public class EnvioTareaController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }


        private readonly ILogger<EnvioTareaController> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IConfiguration _configuration;

        private readonly IAuthorizationService _authorizationService;

        [BindProperty]
        public EnvioTareaViewModel EnviotareaView { get; set; }
        public EnvioTareaController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, ILogger<EnvioTareaController> logger, IConfiguration configuration, IAuthorizationService authorizationService)
        {
            _db = db;

            _logger = logger;

            _configuration = configuration;

            EnviotareaView = new EnvioTareaViewModel()
            {
                EnvioTarea = new EnvioTarea(),

                Calificaciones = _db.Calificaciones,

                EnvioTareaList = _db.EnvioTarea

                
            };

            _authorizationService = authorizationService;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: EnvioTarea
        [Authorize(Roles = SD.StudenUser)]
        public async Task<IActionResult> Create(int? id,int? Calid)
        {
            var doesCalexist = _db.Calificaciones.Include(e => e.Estudiante).Include(p => p.Tareas).Where(p => p.Id == Calid);

            var doesCursoexist = _db.Curso.Where(c => c.Id == id);
            if (Calid == null || doesCalexist.Count() == 0 )
            {
                return RedirectToAction("TareasModule", "Curso", new { id = id });
            }


            if (doesCursoexist.Count() == 0 || id == null)
            {
                return RedirectToAction("TareasModule", "Curso", new { id = id });

            }

            var Cal = await _db.Calificaciones.Include(e => e.Estudiante).Include(t => t.Tareas).Where(e => e.Id == Calid).SingleAsync();


            var result = await _authorizationService.AuthorizeAsync(User, Cal, "CalificacionEnvioTareaAuthorization");

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


            if(Cal.Enviado == 1 || Cal.Tareas.EndDate < DateTime.Now)
            {

                return RedirectToAction("TareasModules", "Curso", new { id = id });
            }


            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);



            //buscar calificacion que tenga el id del estudiante actual, la query debe ser c.tareas.cursoid == id && Estudianteid == claim.value

            //Solo el estudiante tiene acceso al envio,el profesor no,debes esconder la view del envio,para el profesor

            EnviotareaView.Calificaciones = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
                Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value && c.Id == Calid).ToListAsync();

            EnviotareaView.Calificacionesid = await _db.Calificaciones.Where(c => c.Id == Calid).SingleOrDefaultAsync();
            //Calificaciones Calificacion = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
            //    Where(c => c.Tareas.CursoId == id).FirstOrDefaultAsync();


            EnviotareaView.EnvioTarea.CalificacionId = EnviotareaView.Calificacionesid.Id;

            //EnviotareaView.EnvioTarea.Calificacion = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
            //    Where(c => c.Tareas.CursoId == id).FirstOrDefaultAsync();

            //EnviotareaView.EnvioTarea.CalificacionId = Calificacion.Id;


            return View(EnviotareaView);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(int id)
        {

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            //sincroniza con el id de calificacion 

            // int d = EnviotareaView.EnvioTarea.CalificacionId;

            Calificaciones cal = await _db.Calificaciones.Include(t => t.Tareas).ThenInclude(c => c.Cursos).
               Where(c => c.Tareas.CursoId == id && c.Estudiante.UserId == claim.Value &&  c.Id == EnviotareaView.Calificacionesid.Id).FirstOrDefaultAsync();

            EnviotareaView.EnvioTarea.CalificacionId = cal.Id;






            if (!ModelState.IsValid  )
            {
                
                //ver como enviar el mensaje de error ya que no se envia, también ver si se puede mejorar lo de la id de la calificacion
                //poniendo el cal.id en el create get

                StatusMessage = "Error: La extension del archivo no es valida, solo se pueden subir archivos pdf,docx o xlsx:";
                return View(EnviotareaView);


            }
         
            


                string webroothpath = _webHostEnvironment.WebRootPath;
                if (EnviotareaView.EnvioTarea.File.Length > 0)
                {
                    EnviotareaView.EnvioTarea.FileURL = Guid.NewGuid() + Path.GetExtension(EnviotareaView.EnvioTarea.File.FileName);

                    string fileurl = Path.GetFullPath(Path.Combine(webroothpath, "Files"));


             

                using (var filestream = new FileStream(Path.Combine(fileurl, EnviotareaView.EnvioTarea.FileURL), FileMode.Create))
                    {
                        await EnviotareaView.EnvioTarea.File.CopyToAsync(filestream);

                    }

              

            }

            //el cal enviado ,es para editar la variable enviado de la tabla calificaicon, este cambio se hace apra que en la view de actividades
            // se pueda diferenciar de una actividad que ha sido enviado de una que no, siendo 1 el valor de una actividad enviada.
            cal.Enviado = 1;



            _db.EnvioTarea.Add(EnviotareaView.EnvioTarea);







                await _db.SaveChangesAsync();

                return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = EnviotareaView.EnvioTarea.CalificacionId });


            



        }

        [Authorize(Roles = "Profesor,Estudiante")]
        public async Task<IActionResult> TareaDownloader(int id)
        {

            EnviotareaView.EnvioTareaList = await _db.EnvioTarea.Include(c => c.Calificacion).ThenInclude(c => c.Tareas).Where(c => c.CalificacionId == id).ToListAsync();


            EnviotareaView.EnvioTarea = await _db.EnvioTarea.Include(c => c.Calificacion).ThenInclude(c => c.Tareas).Where(c => c.CalificacionId == id).FirstOrDefaultAsync();

            //if (EnviotareaView.LocalDate <= EnviotareaView.EnvioTarea.Calificacion.EndDate)
            //{

            //    EnviotareaView.test = true;
            //}

            return View(EnviotareaView);
        }

        [Authorize(Roles = SD.StudenUser)]
   
        public async Task<IActionResult> Edit(int? id)
        {

            var doesEnvioexist = _db.EnvioTarea.Include(e => e.Calificacion).Where(p => p.Id == id);

            if (id == null || doesEnvioexist.Count() == 0)
            {
                return NotFound();
            }

            var EnvioTarea = await _db.EnvioTarea.Include(e => e.Calificacion).ThenInclude(e => e.Estudiante).Include(e => e.Calificacion).ThenInclude(t => t.Tareas).Where(e => e.Id == id).SingleAsync();

        

            if(EnvioTarea.Calificacion.Tareas.EndDate < DateTime.Now)
            {
                return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = EnvioTarea.CalificacionId });

            }

            var result = await _authorizationService.AuthorizeAsync(User, EnvioTarea, "EnvioTareaAuthorization");

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


            EnviotareaView.EnvioTarea = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();

           
            return View(EnviotareaView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {



            EnvioTarea Envio = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();


            //EnviotareaView.EnvioTarea.CalificacionId = Envio.CalificacionId;
            //Envio.FileURL = EnviotareaView.EnvioTarea.FileURL;

            string url = Envio.FileURL;            /*EnviotareaView.EnvioTarea.FileURL;*/

            //averigua porque no se envia el enviotare.file, ve a al marcador de google: Upload files in ASP.NET Core y toma en cuenta el iformfile
            //de esos ejemplos

            if (!ModelState.IsValid)
            {

                //ver como enviar el mensaje de error ya que no se envia, también ver si se puede mejorar lo de la id de la calificacion
                //poniendo el cal.id en el create get

                StatusMessage = "Error: La extension del archivo no es valida, solo se pueden subir archivos pdf,docx o xlsx:";
                return View(EnviotareaView);


            }

            string webroothpath = _webHostEnvironment.WebRootPath;

            string uploadsFolder = Path.Combine(webroothpath, "Files");

            var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, url);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }



            if (EnviotareaView.EnvioTarea.File.Length > 0)
            {
                EnviotareaView.EnvioTarea.FileURL = Guid.NewGuid() + Path.GetExtension(EnviotareaView.EnvioTarea.File.FileName);

                string fileurl = Path.GetFullPath(Path.Combine(webroothpath, "Files"));




                using (var filestream = new FileStream(Path.Combine(fileurl, EnviotareaView.EnvioTarea.FileURL), FileMode.Create))
                {
                    await EnviotareaView.EnvioTarea.File.CopyToAsync(filestream);

                }



            }

            var test = await _db.EnvioTarea.FindAsync(id);

            test.Descripcion = EnviotareaView.EnvioTarea.Descripcion;

            test.CalificacionId = EnviotareaView.EnvioTarea.CalificacionId;

            test.FileURL = EnviotareaView.EnvioTarea.FileURL;


            await _db.SaveChangesAsync();

          

            return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = EnviotareaView.EnvioTarea.CalificacionId });



        }


        public async Task<string> FileUpload(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }


     


        public FileResult DownloadFile(string fileName)
        {

            string webroothpath = _webHostEnvironment.WebRootPath;
            //Build the File Path.
            string path = Path.Combine(webroothpath, "Files", fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        //[Authorize(Roles = SD.TeacherUser)]
        //public async Task<IActionResult> CalificarTarea(int? id)
        //{

        //    var doesEnvioexist = _db.EnvioTarea.Include(e => e.Calificacion).Where(p => p.Id == id);

        //    if (id == null || doesEnvioexist.Count() == 0)
        //    {
        //        return NotFound();
        //    }


        //    EnviotareaView.EnvioTarea = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();


        //    return View(EnviotareaView);
        //}





        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CalificarTarea(int id)
        //{

   

        //    EnvioTarea Envio = await _db.EnvioTarea.Include(c => c.Calificacion).Where(c => c.Id == id).SingleOrDefaultAsync();


        //    string url = Envio.FileURL; 

        //    if (!ModelState.IsValid)
        //    {

        //        StatusMessage = "Error: La extension del archivo no es valida, solo se pueden subir archivos pdf,docx o xlsx:";
        //        return View(EnviotareaView);


        //    }

        //    string webroothpath = _webHostEnvironment.WebRootPath;

        //    string uploadsFolder = Path.Combine(webroothpath, "Files");

        //    var path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), uploadsFolder, url);

        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }



        //    if (EnviotareaView.EnvioTarea.File.Length > 0)
        //    {
        //        EnviotareaView.EnvioTarea.FileURL = Guid.NewGuid() + Path.GetExtension(EnviotareaView.EnvioTarea.File.FileName);

        //        string fileurl = Path.GetFullPath(Path.Combine(webroothpath, "Files"));




        //        using (var filestream = new FileStream(Path.Combine(fileurl, EnviotareaView.EnvioTarea.FileURL), FileMode.Create))
        //        {
        //            await EnviotareaView.EnvioTarea.File.CopyToAsync(filestream);

        //        }



        //    }

        //    var test = await _db.EnvioTarea.FindAsync(id);

        //    test.Descripcion = EnviotareaView.EnvioTarea.Descripcion;

        //    test.CalificacionId = EnviotareaView.EnvioTarea.CalificacionId;

        //    //test.FileURL = EnviotareaView.EnvioTarea.FileURL;


        //    await _db.SaveChangesAsync();



        //    return RedirectToAction("TareaDownloader", "EnvioTarea", new { id = EnviotareaView.EnvioTarea.CalificacionId });



        //}

    



    }
}
