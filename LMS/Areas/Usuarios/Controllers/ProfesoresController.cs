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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Usuarios.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Usuarios")]
    public class ProfesoresController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserManager<ApplicationUser> userManager;
        [TempData]
        public string StatusMessage { get; set; }

        public ProfesoresController(ApplicationDbContext db)
        {
            _db = db;
        }

    
        public async Task<IActionResult> Index()
        {
            var Profesores = await _db.Profesores.Include(p => p.ApplicationUser).Include(i => i.MateriaProfesor).Include(i => i.ClaseProfesor).ToListAsync();

          

            //viewmodel.Profesores = await _db.Profesores.Include(i => i.ClaseProfesor).Include(i => i.MateriaProfesor)
            //    .ToListAsync();

            
            return View(Profesores);
        }

        //Create-GET
        public async Task<IActionResult> Create()
        {
            var item = _db.Clase.ToList();

            var mat = _db.Materia.ToList();




        

            //var asd = await (from user in _db.ApplicationUser
            //                 join userRole in _db.UserRoles
            //                 on user.Id equals userRole.UserId
            //                 join rol in _db.Roles
            //                 on userRole.RoleId equals rol.Id
            //                 where rol.Name == "PROFESOR"
            //                 select user).ToListAsync();

            UsuarioyClaseViewModel model = new UsuarioyClaseViewModel()
            {
                //UsuariosList = await _db.ApplicationUser.ToListAsync(),

             

            Profesores = new Models.Profesores(),

            
            };


            model.UsuariosList = await (from user in _db.ApplicationUser
                                        join userRole in _db.UserRoles
                                        on user.Id equals userRole.UserId
                                        join rol in _db.Roles
                                        on userRole.RoleId equals rol.Id
                                        where rol.Name == "PROFESOR"
                                        select user).ToListAsync();

            model.ClasesDisponibles = item.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList(); 

            model.MateriasDisponibles = mat.Select(m => new CheckBoxItem()

            {
                Id = m.Id,
                Title = m.Nombre,

                IsChecked = false
            }).ToList() ;

            

            return View(model);
        }

        //Create-Post

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(UsuarioyClaseViewModel model)
        {
            List<ClaseProfesor> stc = new List<ClaseProfesor>();

            List<MateriaProfesor> mtc = new List<MateriaProfesor>();



            if (ModelState.IsValid)
            {
                var doesProfesorexist = _db.Profesores.Include(p => p.ApplicationUser)/*.Include(p => p.Clase)*//*.Include(p => p.Materia)*/.Where(p => p.ApplicationUser.Id == model.Profesores.UserId /*&& p.Clase.Id == model.Profesores.ClaseId*/ /*&& p.Materia.Id == model.Profesores.MateriaId*/);

                if(doesProfesorexist.Count() > 0)
                {
                    StatusMessage = "Error: Ese Profesor ya existe bajo el nombre de:" + doesProfesorexist.First().ApplicationUser.Nombre + ",por favor, use otro usuario";
                }
                else
                {
                  

                    _db.Profesores.Add(model.Profesores);

                    await _db.SaveChangesAsync();


                    foreach (var item in model.ClasesDisponibles)
                    {
                        if (item.IsChecked == true)
                        {
                            stc.Add(new ClaseProfesor() { ProfesorId = model.Profesores.Id, ClaseId = item.Id });
                        }
                    }

                    foreach (var item in stc)
                    {
                        var cn = _db.Clase.Where(c => c.Id == item.ClaseId).FirstOrDefault();
                        item.NombreClase = cn.Nombre;
                        _db.ClaseProfesors.Add(item);

                       
                    }


                    foreach(var materia in model.MateriasDisponibles)
                    {
                        if(materia.IsChecked == true)
                        {
                            mtc.Add(new MateriaProfesor() { ProfesorId = model.Profesores.Id, MateriaId = materia.Id });
                        }
                    }

                    foreach(var materia in mtc)
                    {

                        var mt = _db.Materia.Where(c => c.Id == materia.MateriaId).FirstOrDefault();

                        materia.NombreMateria = mt.Nombre;
                        _db.MateriaProfesors.Add(materia);

                       
                    }

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            UsuarioyClaseViewModel modelVM = new UsuarioyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Profesores = model.Profesores,

            

                //MateriaList = await _db.Materia.ToListAsync(),

                //ClaseList = await _db.Clase.ToListAsync(),

               StatusMessage = StatusMessage
            };

            var tem = _db.Materia.ToList();
            var mat = _db.Materia.ToList();

            modelVM.ClasesDisponibles = tem.Select(vm => new CheckBoxItem()
            {
                Id = vm.Id,
                Title = vm.Nombre,
                IsChecked = false

            }).ToList();

            modelVM.MateriasDisponibles = mat.Select(m => new CheckBoxItem()

            {
                Id = m.Id,
                Title = m.Nombre,

                IsChecked = false
            }).ToList();


            return View(modelVM);
        }


        //Edit-GET
        public async Task<IActionResult> Edit(int ?id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var Profesor = await _db.Profesores.Include(s => s.ClaseProfesor).ThenInclude(e => e.Clases).AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if(Profesor == null)
            {
                return NotFound();
            }


           
            UsuarioyClaseViewModel model = new UsuarioyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Profesores =  Profesor,

                //MateriaList = await _db.Materia.ToListAsync(),

                //ClaseList = await _db.Clase.ToListAsync()
            };

            var todaslasclases = _db.Clase.Select(c => new CheckBoxItem()
            {

                Id = c.Id,
                Title = c.Nombre,
                IsChecked = c.ClaseProfesor.Any(x => x.ProfesorId == id) ? true : false

            }).ToList();

            model.ClasesDisponibles = todaslasclases;

            var todaslasmaterias = _db.Materia.Select(m => new CheckBoxItem()
            {
                Id = m.Id,
                Title = m.Nombre,
                IsChecked = m.MateriaProfesor.Any(z => z.ProfesorId == id) ? true : false
            }).ToList();

            model.MateriasDisponibles = todaslasmaterias;

            return View(model);
        }

        //Edit-Post

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id,UsuarioyClaseViewModel model, ClaseProfesor ProfesorC)
        {
            List<ClaseProfesor> stc = new List<ClaseProfesor>();

            List<MateriaProfesor> mtc = new List<MateriaProfesor>();

            
         

            if (ModelState.IsValid)
            {
                
                    var ProfFromDB = await _db.Profesores.FindAsync(id);

                    //ProfFromDB.ClaseId = model.Profesores.ClaseId;

                    //ProfFromDB.MateriaId = model.Profesores.MateriaId;

            

                    await _db.SaveChangesAsync();

                foreach (var materia in model.MateriasDisponibles)
                {
                    if (materia.IsChecked == true)
                    {
                        mtc.Add(new MateriaProfesor() { ProfesorId = id, MateriaId = materia.Id });
                    }
                }

                
                var tabledata = _db.MateriaProfesors.Where(m => m.ProfesorId == id).ToList();

                var listresult = tabledata.Except(mtc).ToList();

                foreach(var mat in listresult)
                {
                    _db.MateriaProfesors.Remove(mat);
                    _db.SaveChanges();
                }

                var getmateriaid = _db.MateriaProfesors.Where(m => m.ProfesorId == id).ToList();

                foreach (var m in mtc)
                {
                    if (!getmateriaid.Contains(m))
                    {
                        var mt = _db.Materia.Where(c => c.Id == m.MateriaId).FirstOrDefault();

                        m.NombreMateria = mt.Nombre;


                        _db.MateriaProfesors.Add(m);
                        _db.SaveChanges();
                    }
                }


                foreach (var item in model.ClasesDisponibles)
                    {
                        if (item.IsChecked == true)
                        {
                            stc.Add(new ClaseProfesor() { ProfesorId = id, ClaseId = item.Id });
                        }
                    }

                    var databasetable = _db.ClaseProfesors.Where(a => a.ProfesorId == id).ToList();

                    var resultlist = databasetable.Except(stc).ToList();

                    foreach (var item in resultlist)
                    {
                        _db.ClaseProfesors.Remove(item);
                        _db.SaveChanges(); 
                    }

                    var getclaseid = _db.ClaseProfesors.Where(a => a.ProfesorId == id).ToList();

                    foreach (var item in stc)
                    {
                        if (!getclaseid.Contains(item))
                        {   
                            var cn = _db.Clase.Where(c => c.Id == item.ClaseId).FirstOrDefault();
                        item.NombreClase = cn.Nombre;
                            
                        _db.ClaseProfesors.Add(item);
                            _db.SaveChanges();
                        }
                    }

                    return RedirectToAction(nameof(Index));
                
            }

            UsuarioyClaseViewModel modelVM = new UsuarioyClaseViewModel()
            {
                UsuariosList = await _db.ApplicationUser.ToListAsync(),

                Profesores = model.Profesores,

                //MateriaList = await _db.Materia.ToListAsync(),

                //ClaseList = await _db.Clase.ToListAsync(),

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
            var Profesor = await _db.Profesores.Include(p => p.ApplicationUser)/*.Include(p => p.Clase)*//*.Include(p => p.Materia)*/.SingleOrDefaultAsync(m => m.Id == id);
            if (Profesor == null)
            {
                return NotFound();
            }

            return View(Profesor);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var profesor = await _db.Profesores.SingleOrDefaultAsync(m => m.Id == id);
            _db.Profesores.Remove(profesor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

    }
}