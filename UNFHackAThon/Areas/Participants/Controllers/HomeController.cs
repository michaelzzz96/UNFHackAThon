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
                HttpContext.Session.SetInt32("ssCartCount", cnt);
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
                    await _db.CompetitionCart.AddAsync(CartObject);
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


        public ActionResult Code()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Code(IFormFile[] files)
        {

            try
            {
                // Iterate through uploaded files array
                foreach (var file in files)
                {
                    // Extract file name from whatever was posted by browser
                    // relative path, "MyFile.jpg"
                    //var basePath = "C:\\temp\\";
                    var fileName = System.IO.Path.GetFileName(file.FileName);


                    // If file with same name exists delete it
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    // Create new local file and copy contents of uploaded file
                    // goal: basePath\CompetitionId\UserId\file.FileName
                    // database row has Columns for Path=CompetitionId\UserId\RandomId.jpg and OriginalName=file.FileName

                    // if fileName is Absolute, then use it as is and open the file mentioned
                    // if fileName is relative... then use the current execution directory as the root folder, and then open a file at root + relative
                    // System.IO.File.OpenWrite(@"C:\temp\myfile.jpg") -- open C:\temp\myfile.jpg
                    // System.IO.File.OpenWrite(@"myfile.jpg") -- ?what folder? - whenever your code is
                    using (var localFile = System.IO.File.OpenWrite(fileName))
                    using (var uploadedFile = file.OpenReadStream())
                    {
                        uploadedFile.CopyTo(localFile);
                    }
                }

                ViewBag.Message = "Files successfully uploaded";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Failed to upload, please try again.";
            }

            return View();
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