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
    public class PlayerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public PlayerController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Admin/Player
        public async Task<IActionResult> Index()
        {
              return View(await _dbContext.Player.ToListAsync());
        }

        // GET: Admin/Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _dbContext.Player == null)
            {
                return NotFound();
            }

            var player = await _dbContext.Player
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Admin/Player/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,RegistrationId,Birthday")] Player player)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(player);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Admin/Player/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _dbContext.Player == null)
            {
                return NotFound();
            }

            var player = await _dbContext.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Admin/Player/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,RegistrationId,Birthday")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(player);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        // GET: Admin/Player/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _dbContext.Player == null)
            {
                return NotFound();
            }

            var player = await _dbContext.Player
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Admin/Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_dbContext.Player == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Player'  is null.");
            }
            var player = await _dbContext.Player.FindAsync(id);
            if (player != null)
            {
                _dbContext.Player.Remove(player);
            }
            
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
          return _dbContext.Player.Any(e => e.Id == id);
        }
    }
}
