using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;

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
    }
}