using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Register.Data;
using Register.Models;

namespace Register.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize()]
    public class TeamController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public TeamController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int clubId)
        {
            if (CheckClubPermission(clubId))
            {
                var teams = _dbContext.Team.Where(t => t.ClubId == clubId).ToList();
                foreach (var team in teams)
                {
                    team.Season = _dbContext.Season.FirstOrDefault(s => s.Id == team.SeasonId).Year;
                }
                return View(teams);
            };

            return RedirectToAction("Club", "Club");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(User);
            if (_dbContext.Club.FirstOrDefault(c => c.UserId == userId) != null)
            {
                ViewBag.ClubId = _dbContext.Club.FirstOrDefault(c => c.UserId == userId).Id;
            }
            else
            {
                return RedirectToAction("Club", "Club");
            }
            
            ViewBag.Seasons = _dbContext.Season.Select(s => new SelectListItem() { Value = s.Id.ToString(), Text = s.Year });
            ViewBag.UserId = userId;            
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Team.Add(team);
                    _dbContext.SaveChanges();
                    
                    return RedirectToAction("Index", new { id = team.ClubId });
                }
                catch (Exception e)
                {
                    if (e.InnerException.Message != null)
                    {
                        TempData["Error"] = e.InnerException.Message;
                    }
                    else
                    {
                        TempData["Error"] = e.Message;
                    }
                    
                    return RedirectToAction("Index", new { id = team.ClubId });
                }
            }

            return View(team);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (CheckTeamPermission(id))
            {
                if (_dbContext.Team.Find(id) == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var teamPlayers = _dbContext.TeamPlayer.Where(t => t.TeamId == id).Select(tp => new TeamPlayer
                {
                    Id = tp.Id,
                    TeamId = tp.TeamId,
                    PlayerId = tp.PlayerId,
                    PlayerName = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).FirstName + " " + _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).LastName,
                    PlayerBirthday = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).Birthday,
                    PlayerRegistrationId = _dbContext.Player.FirstOrDefault(p => p.Id == tp.PlayerId).RegistrationId
                }).ToList();


                var teamPersonnel = _dbContext.TeamPersonnel.Where(t => t.TeamId == id).Select(tp => new TeamPersonnel
                {
                    Id = tp.Id,
                    TeamId = tp.TeamId,
                    PersonnelId = tp.PersonnelId,
                    PersonnelName = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).FirstName + " " + _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).LastName,
                    PersonnelBirthday = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).Birthday,
                    PersonnelRegistrationId = _dbContext.Personnel.FirstOrDefault(p => p.Id == tp.PersonnelId).RegistrationId
                }).ToList();

                ViewBag.TeamPersonnel = teamPersonnel;
                ViewBag.TeamId = id;
                ViewBag.ClubId = _dbContext.Team.Find(id).ClubId;

                return View(teamPlayers);
            }
            var clubId = _dbContext.Team.Find(id).ClubId;
            return RedirectToAction("Index", new { id = clubId });

        }

        [HttpGet]
        public IActionResult AddTeamPlayer(int id)
        {
            if (CheckTeamPermission(id))
            {
                if (_dbContext.Team.Find(id) == null)
                {
                    return RedirectToAction("Index", new { id = id });
                }

                var teamToEdit = _dbContext.Team.FirstOrDefault(t => t.Id == id);
                teamToEdit.Season = _dbContext.Season.FirstOrDefault(s => s.Id == teamToEdit.SeasonId).Year;

                ViewBag.Players = _dbContext.Player.ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.FirstName + " " + p.LastName + " - " + p.RegistrationId + " - " + p.Birthday.Year });
                ViewBag.TeamId = teamToEdit.Id;

                return View();
            }

            return RedirectToAction("Index", new { id = id });
        }

        [HttpGet]
        public IActionResult RemoveTeamPlayer(int id)
        {
            var teamPlayer = _dbContext.TeamPlayer.FirstOrDefault(p => p.Id == id);
            _dbContext.TeamPlayer.Remove(teamPlayer);
            _dbContext.SaveChanges();

            return RedirectToAction("Edit",new { id = teamPlayer.TeamId });
        }

        [HttpPost]
        public IActionResult ConfirmAddTeamPlayer(TeamPlayer teamPlayer)
        {
            _dbContext.TeamPlayer.Add(teamPlayer);
            _dbContext.SaveChanges();

            return RedirectToAction("Edit", new { id = teamPlayer.TeamId });
        }


        [HttpGet]
        public IActionResult AddTeamPersonnel(int id)
        {
            if (CheckTeamPermission(id))
            {
                if (_dbContext.Team.Find(id) == null)
                {
                    return RedirectToAction("Index", new { id = id });
                }

                var teamToEdit = _dbContext.Team.FirstOrDefault(t => t.Id == id);
                teamToEdit.Season = _dbContext.Season.FirstOrDefault(s => s.Id == teamToEdit.SeasonId).Year;

                ViewBag.Personnel = _dbContext.Personnel.ToList().Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.FirstName + " " + p.LastName + " - " + p.RegistrationId + " - " + p.Birthday.Year });
                ViewBag.TeamId = teamToEdit.Id;

                return View();
            }

            return RedirectToAction("Index", new { id = id });
        }

        [HttpPost]
        public IActionResult ConfirmAddTeamPersonnel(TeamPersonnel teamPersonnel)
        {
            _dbContext.TeamPersonnel.Add(teamPersonnel);
            _dbContext.SaveChanges();

            return RedirectToAction("Edit", new { id = teamPersonnel.TeamId });
        }


        [HttpGet]
        public IActionResult RemoveTeamPersonnel(int id)
        {
            var teamPersonnel = _dbContext.TeamPersonnel.FirstOrDefault(p => p.Id == id);
            _dbContext.TeamPersonnel.Remove(teamPersonnel);
            _dbContext.SaveChanges();

            return RedirectToAction("Edit", new { id = teamPersonnel.TeamId });
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (CheckTeamPermission(id))
            {
                var team = _dbContext.Team.FirstOrDefault(t => t.Id == id);;
                return View(team);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (_dbContext.Team.Find(id) == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var team = _dbContext.Team.FirstOrDefault(t => t.Id == id);
            _dbContext.Team.Remove(team);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
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

        private bool CheckTeamPermission(int teamId)
        {
            var userId = _userManager.GetUserId(User);
            if (_dbContext.Team.Any(c => c.Id == teamId && c.UserId == userId))
            {
                return true;
            }

            return false;
        }


    }
}
