using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Register.Data;
using Register.Models;

namespace Register.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PersonnelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PersonnelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Personnel
        public async Task<IActionResult> Index()
        {
              return View(await _context.Personnel.ToListAsync());
        }

        // GET: Admin/Personnel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Personnel == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // GET: Admin/Personnel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Personnel/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,RegistrationId,Birthday,Role")] Personnel personnel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personnel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personnel);
        }

        // GET: Admin/Personnel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Personnel == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel == null)
            {
                return NotFound();
            }
            return View(personnel);
        }

        // POST: Admin/Personnel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,RegistrationId,Birthday,Role")] Personnel personnel)
        {
            if (id != personnel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personnel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonnelExists(personnel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personnel);
        }

        // GET: Admin/Personnel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Personnel == null)
            {
                return NotFound();
            }

            var personnel = await _context.Personnel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personnel == null)
            {
                return NotFound();
            }

            return View(personnel);
        }

        // POST: Admin/Personnel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Personnel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Personnel'  is null.");
            }
            var personnel = await _context.Personnel.FindAsync(id);
            if (personnel != null)
            {
                _context.Personnel.Remove(personnel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonnelExists(int id)
        {
          return _context.Personnel.Any(e => e.Id == id);
        }
    }
}
