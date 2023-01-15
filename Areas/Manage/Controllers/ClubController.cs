using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Register.Controllers;
using Register.Data;
using Register.Models;
using System.Diagnostics;

namespace Register.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize]
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ClubController> _logger;
        private readonly ILogger _factoryLogger;

        public ClubController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<ClubController> logger, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
            _factoryLogger = loggerFactory.CreateLogger("DataAccessLayer");
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
            _logger.LogWarning(String.Format("User {0} attempted to browse to an unowned club.", _userManager.GetUserId(User)));
            return RedirectToAction(nameof(Club));
        }

        [HttpPost]
        public IActionResult Edit(Club club)
        {
            if (ModelState.IsValid)
            {
                var timer = new Stopwatch();

                timer.Start();
                _dbContext.Club.Update(club);
                _dbContext.SaveChanges();
                timer.Stop();

                _logger.LogInformation(String.Format("Club {0} was edited successfully. Edit was done by user {1} - {2}. Operation took {3} ms to complete", club.Name, _userManager.GetUserId(User), _userManager.GetUserName(User), timer.ElapsedMilliseconds));

                _factoryLogger.LogInformation(String.Format("This is logged by Factory logger. Club {0} was edited successfully. Edit was done by user {1} - {2}. Operation took {3} ticks to complete", club.Name, _userManager.GetUserId(User), _userManager.GetUserName(User), timer.ElapsedTicks));

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
