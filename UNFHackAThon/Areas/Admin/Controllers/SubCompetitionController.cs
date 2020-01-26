using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models.ViewModels;

namespace UNFHackAThon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCompetitionController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SubCompetitionController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET INDEX
        public async  Task<IActionResult> Index()
        {
            var subCompetition = await _db.SubCompetition.Include(s=>s.Competition).ToListAsync();
            return View(subCompetition);

        }

        //GET - CREATE
        public async Task<IActionResult> Create()
        {
            SubCompetitionAndCompetitionViewModel model = new SubCompetitionAndCompetitionViewModel()
            {
                CompetitionList = await _db.Competition.ToListAsync(),
                SubCompetition = new Models.SubCompetition(),
                SubCompetitionList = await _db.SubCompetition.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }


    }
}