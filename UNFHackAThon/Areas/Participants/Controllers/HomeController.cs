using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models;
using UNFHackAThon.Models.ViewModels;
using UNFHackAThon.Utility;

namespace UNFHackAThon.Controllers
{
    [Authorize()]
    [Area("Participants")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                CompetitionItem = await _db.CompetitionItem.Include(m => m.Competition).Include(m => m.SubCompetition).ToListAsync(),
                Competition = await _db.Competition.ToListAsync(),
            };


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var cnt = _db.CompetitionCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32("ssCartCount",cnt);
            }


            return View(IndexVM);
        }


        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var competitionItemFromDb = await _db.CompetitionItem.Include(m => m.Competition).Include(m => m.SubCompetition).Where(m => m.Id == id).FirstOrDefaultAsync();

            CompetitionCart cartObj = new CompetitionCart()
            {
                CompetitionItem = competitionItemFromDb,
                CompetitionItemId = competitionItemFromDb.Id
            };

            return View(cartObj);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(CompetitionCart CartObject)
        {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                CompetitionCart cartFromDb = await _db.CompetitionCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId 
                                                  && c.CompetitionItemId == CartObject.CompetitionItemId).FirstOrDefaultAsync();
                if (cartFromDb == null)
                {
                  await  _db.CompetitionCart.AddAsync(CartObject);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;
                }
               await _db.SaveChangesAsync();

                var count = _db.CompetitionCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32("ssCartCount", count);

                return RedirectToAction("Index");
            }
            else
            {
                var competitionItemFromDb = await _db.CompetitionItem.Include(m => m.Competition).Include(m => m.SubCompetition).Where(m => m.Id == CartObject.CompetitionItemId).FirstOrDefaultAsync();

                CompetitionCart cartObj = new CompetitionCart()
                {
                    CompetitionItem = competitionItemFromDb,
                    CompetitionItemId = competitionItemFromDb.Id
                };

                return View(cartObj);
            }
        }

     
        public ActionResult Code( )
        {

            return View();
        }


        public class IndexModel : PageModel
        {

            public IEnumerable<string> ImageFiles { get; set; }

            public void OnGet([FromServices]IHostingEnvironment env)
            {
                string imagePath =
        $"{env.WebRootPath}\\images";

                this.ImageFiles = Directory.GetFiles
        (imagePath).Select(fileName => Path.GetFileName(fileName));
            }
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
