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
    public class ClasesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ClasesController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Clase.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [PreventDoublePost]
        public async Task<IActionResult> Create(Clase clase)
        {
            if (ModelState.IsValid)
            {
                _db.Clase.Add(clase);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(clase);
        }


        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Clase.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Clase clase)
        {
            if (ModelState.IsValid)
            {
                _db.Update(clase);

                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(clase);
        }

        //Get delete

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _db.Clase.FindAsync(id);
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

            var category = await _db.Clase.FindAsync(id);

            if (category == null)
            {
                return View();
            }

            _db.Clase.Remove(category);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


    }
}