using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Register.Data;
using Register.Models;

namespace Register.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SeasonController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public SeasonController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var seasons = _dbContext.Season.ToList();
            return View(seasons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Season season)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Season.Add(season);
                _dbContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(season);
        }
    }
}
