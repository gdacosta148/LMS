using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data;
using LMS.Extensions;
using LMS.Models;
using LMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Areas.Usuarios.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Usuarios")]
    public class MateriaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MateriaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Materia.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
       
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _db.Materia.Add(materia);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(materia);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Materia.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Materia materia)
        {
            if (ModelState.IsValid)
            {
                _db.Update(materia);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(materia);
        }

        //Get delete

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Materia.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //Delete-post


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var category = await _db.Materia.FindAsync(id);

            if (category == null)
            {
                return View();
            }

            _db.Materia.Remove(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }



    }
}