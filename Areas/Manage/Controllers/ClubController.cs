using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Register.Data;
using Register.Models;

namespace Register.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ClubController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Club()
        {
            var userId = _userManager.GetUserId(User);

            if (userId != null)
            {
                if (_dbContext.Club.FirstOrDefault(c => c.UserId == userId) == null)
                {
                    return View();
                }
                var club = _dbContext.Club.FirstOrDefault(c => c.UserId == userId);
                var user = await _userManager.FindByIdAsync(userId);
                club.Administrator = user.UserName;
                return View(club);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.UserId = _userManager.GetUserId(User);
            if (_dbContext.Club.Any(c => c.UserId == userId))
            {
                return RedirectToAction(nameof(Club));
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("Name", "City", "UserId")]Club club)
        {
            club.Administrator = _dbContext.Users.FirstOrDefault(u => u.Id == club.UserId).UserName;
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Club.Add(club);
                    _dbContext.SaveChanges();
                    return RedirectToAction(nameof(Club));
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException.Message != null)
                    {
                        TempData["Error"] = e.InnerException.Message;
                    }
                    else
                    {
                        TempData["Error"] = e.Message;
                    }

                    return RedirectToAction(nameof(Club));
                }
            }
            return View(club);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (CheckClubPermission(id))
            {
                var club = _dbContext.Club.FirstOrDefault(c => c.Id == id);
                return View(club);
            }
            return RedirectToAction(nameof(Club));
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Club.Update(club);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Club));
            }

            return View(club);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (CheckClubPermission(id))
            {
                var club = _dbContext.Club.FirstOrDefault(c => c.Id == id);
                club.Administrator = _dbContext.Users.FirstOrDefault(u => u.Id == club.UserId).UserName;
                return View(club);
            }
            return RedirectToAction(nameof(Club));
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (_dbContext.Club.Find(id) == null)
            {
                return RedirectToAction(nameof(Club));
            }

            var club = _dbContext.Club.FirstOrDefault(c => c.Id == id);
            _dbContext.Club.Remove(club);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Club));
        }

        private bool CheckClubPermission(int clubId)
        {
            var userId = _userManager.GetUserId(User);
            if (_dbContext.Club.Any(c => c.Id == clubId && c.UserId == userId))
            {
                return true;
            }

            return false;
        }
    }
}
