using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models.ViewModels;

namespace UNFHackAThon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompetitionItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public CompetitionItemViewModel CompetitionItemVM { get; set; }


        public CompetitionItemController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            CompetitionItemVM = new CompetitionItemViewModel()
            {
                Competition = _db.Competition,
                CompetitionItem = new Models.CompetitionItem()
            };
        }

        public async Task<IActionResult> Index()
        {
            var competitionItems = await _db.CompetitionItem.Include(m=>m.Competition).Include(m => m.SubCompetition).ToListAsync();
            return View(competitionItems);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(CompetitionItemVM);
        }
    }
}