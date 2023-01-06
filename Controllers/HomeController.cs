using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Register.Data;
using Register.Models;
using System.Diagnostics;

namespace Register.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index(string searchText, int season)
        {
            ViewBag.Seasons = _dbContext.Season.Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.Year });
            if (searchText != "" && searchText != null)
            {
                var clubs = _dbContext.Club.Where(c => c.Name.Contains(searchText.Trim()));
                var teams = new List<Team>();

                foreach (var club in clubs)
                {
                    teams.AddRange(_dbContext.Team.Where(t => t.ClubId == club.Id && t.SeasonId == season));
                }
                foreach (var team in teams)
                {
                    team.ClubName = _dbContext.Club.FirstOrDefault(c => c.Id == team.ClubId).Name;
                    team.Season = _dbContext.Season.FirstOrDefault(s => s.Id == team.SeasonId).Year;
                }

                return View(teams);
            }

            return View();
        }

        public IActionResult TeamDetails(int id)
        {
            var teamPlayers = _dbContext.TeamPlayer.Where(tp => tp.TeamId == id).ToList();
            var teamPersonnel = _dbContext.TeamPersonnel.Where(tp => tp.TeamId == id).ToList();

            foreach (var tp in teamPlayers)
            {
                tp.PlayerName = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).FirstName + " " + _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).LastName;
                tp.PlayerBirthday = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).Birthday;
                tp.PlayerRegistrationId = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).RegistrationId;
            }

            foreach (var tp in teamPersonnel)
            {
                tp.PersonnelName = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).FirstName + " " + _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).LastName;
                tp.PersonnelBirthday = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).Birthday;
                tp.PersonnelRegistrationId = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).RegistrationId;
            }

            ViewBag.Personnel = teamPersonnel;

            return View(teamPlayers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}