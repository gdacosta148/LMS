using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data;
using LMS.Extensions;
using LMS.Models;
using LMS.Models.ViewModels;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Usuarios.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Usuarios")]
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

        public EstudiantesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var Estudiantes = await _db.Estudiantes.Include(p => p.ApplicationUser).Include(p => p.Clase).ToListAsync();

            return View(Estudiantes);
        }

        //Create-GET
        public async Task<IActionResult> Create()
        {
            EstudianteyClaseViewModel model = new EstudianteyClaseViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Estudiantes = new Models.Estudiantes(),

                

                ClaseList = await _db.Clase.ToListAsync()
            };

            //model.Estudiantes.NombreEstudiante = model.Estudiantes.ApplicationUser.Nombre;

            model.UsuariosList = await (from user in _db.ApplicationUser
                                        join userRole in _db.UserRoles
                                        on user.Id equals userRole.UserId
                                        join rol in _db.Roles
                                        on userRole.RoleId equals rol.Id
                                        where rol.Name == "ESTUDIANTE"
                                        select user).ToListAsync();

            return View(model);
        }

        //Create-Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(EstudianteyClaseViewModel model)
        {
            ApplicationUser Nombreusuario= await _db.ApplicationUser.Where(p => p.Id == model.Estudiantes.UserId).FirstOrDefaultAsync(); 
            if (ModelState.IsValid)
            {
                var doesEstudianteExist = _db.Estudiantes.Include(p => p.ApplicationUser).Include(p => p.Clase).Where(p => p.ApplicationUser.Id == model.Estudiantes.UserId && p.Clase.Id == model.Estudiantes.ClaseId );

                if (doesEstudianteExist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Estudiante ya existe bajo el nombre de:" + doesEstudianteExist.First().ApplicationUser.Nombre + ",por favor, use otro usuario";
                }
                else
                {
                    

                    model.Estudiantes.NombreEstudiante = Nombreusuario.Nombre;
                    //model.Estudiantes.NombreEstudiante = model.Estudiantes.ApplicationUser.Nombre;
                    _db.Estudiantes.Add(model.Estudiantes);

                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            EstudianteyClaseViewModel modelVM = new EstudianteyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Estudiantes = model.Estudiantes,

         
                ClaseList = await _db.Clase.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }


        //Edit-GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Estudiante = await _db.Estudiantes.Include(e => e.ApplicationUser).Include(e => e.Clase).SingleOrDefaultAsync(m => m.Id == id);

            if (Estudiante == null)
            {
                return NotFound();
            }

            EstudianteyClaseViewModel model = new EstudianteyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Estudiantes = Estudiante,

             

                ClaseList = await _db.Clase.ToListAsync()
            };

            return View(model);
        }

        //Edit-Post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, EstudianteyClaseViewModel model)
        {

            if (ModelState.IsValid)
            {
                var doesEstudianteexist = _db.Estudiantes.Include(p => p.ApplicationUser).Include(p => p.Clase).Where(p => p.ApplicationUser.Id == model.Estudiantes.UserId && p.Clase.Id == model.Estudiantes.ClaseId);

                if (doesEstudianteexist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Estudiante ya existe bajo el nombre de:" + doesEstudianteexist.First().ApplicationUser.Nombre + ",por favor, use otro usuario";
                }
                else
                {
                    var EstFromDB = await _db.Estudiantes.FindAsync(id);

                    EstFromDB.ClaseId = model.Estudiantes.ClaseId;

                    EstFromDB.NombreEstudiante = model.Estudiantes.NombreEstudiante;



                    await _db.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            EstudianteyClaseViewModel modelVM = new EstudianteyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Estudiantes = model.Estudiantes,

                ClaseList = await _db.Clase.ToListAsync(),

                StatusMessage = StatusMessage
            };

            return View(modelVM);
        }

        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Estudiante = await _db.Estudiantes.Include(p => p.ApplicationUser).Include(p => p.Clase).SingleOrDefaultAsync(m => m.Id == id);
            if (Estudiante == null)
            {
                return NotFound();
            }

            return View(Estudiante);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _db.Estudiantes.SingleOrDefaultAsync(m => m.Id == id);
            _db.Estudiantes.Remove(estudiante);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}