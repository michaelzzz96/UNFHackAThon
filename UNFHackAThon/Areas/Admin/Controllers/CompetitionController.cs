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
    public class CompetitionController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CompetitionController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET 
        public async Task<IActionResult> Index()
        {
            return View(await _db.Competition.ToListAsync());
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }
    }
}