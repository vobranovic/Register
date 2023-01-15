using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Register.Data;
using Register.Models;

namespace Register.Controllers
{
    [Authorize]
    [Area("Manage")]
    public class LookupController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public LookupController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string searchText, string searchFor)
        {

            if (searchFor == "Player" && searchText != "" && searchText != null)
            {
                var players = _dbContext.Player.Where(p => p.FirstName.Contains(searchText.Trim()) || p.LastName.Contains(searchText.Trim())).ToList();
                foreach (var player in players)
                {
                    player.NumberOfTeams = _dbContext.TeamPlayer.Count(tp => tp.PlayerId == player.Id);
                }
                return View(players);
            }
            else if (searchFor == "Personnel" && searchText != "" && searchText != null)
            {
                var personnel = _dbContext.Personnel.Where(p => p.FirstName.Contains(searchText.Trim()) || p.LastName.Contains(searchText.Trim())).ToList();
                foreach (var item in personnel)
                {
                    item.NumberOfTeams = _dbContext.TeamPersonnel.Count(tp => tp.PersonnelId == item.Id);
                }

                return View("PersonnelResult", personnel);
            }

            return View();
        }

        public IActionResult PlayerDetails(int id)
        {
            if (_dbContext.Player.FirstOrDefault(p => p.Id == id) != null)
            {
                var player = _dbContext.Player.FirstOrDefault(p => p.Id == id);

                var tps = _dbContext.TeamPlayer.Where(tp => tp.PlayerId == player.Id).AsNoTracking();
                foreach (var item in tps)
                {
                    player.TeamPlayer.Add(item);
                }
                foreach (var tp in player.TeamPlayer)
                {
                    tp.TeamCategory = _dbContext.Team.FirstOrDefault(t => t.Id == tp.TeamId).Category;
                    tp.TeamSeason = _dbContext.Season.FirstOrDefault(s => s.Id == _dbContext.Team.FirstOrDefault(t => t.Id == tp.TeamId).SeasonId).Year;
                    tp.ClubName = _dbContext.Club.FirstOrDefault(c => c.Id == _dbContext.Team.FirstOrDefault(t => t.Id == tp.TeamId).ClubId).Name;
                }

                return View(player);
            }
            
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PersonnelDetails(int id)
        {
            var player = _dbContext.Personnel.FirstOrDefault(p => p.Id == id);
            return View();
        }
        
    }
}
