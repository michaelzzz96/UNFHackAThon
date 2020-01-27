using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models.ViewModels;
using UNFHackAThon.Utility;

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
            var competitionItems = await _db.CompetitionItem.Include(m => m.Competition).Include(m => m.SubCompetition).ToListAsync();
            return View(competitionItems);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View(CompetitionItemVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            CompetitionItemVM.CompetitionItem.SubCompetitionId = Convert.ToInt32(Request.Form["SubCompetitionId"].ToString());
            if (!ModelState.IsValid)
            {
                return View(CompetitionItemVM);
            }
            _db.CompetitionItem.Add(CompetitionItemVM.CompetitionItem);
            await _db.SaveChangesAsync();
            //Work on the image saving section
            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var menuItemFromDb = await _db.CompetitionItem.FindAsync(CompetitionItemVM.CompetitionItem.Id);
            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, CompetitionItemVM.CompetitionItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + CompetitionItemVM.CompetitionItem.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultCompetitionImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + CompetitionItemVM.CompetitionItem.Id + ".png");
                menuItemFromDb.Image = @"\images\" + CompetitionItemVM.CompetitionItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompetitionItemVM.CompetitionItem = await _db.CompetitionItem.Include(m => m.Competition).Include(m => m.SubCompetition).SingleOrDefaultAsync(m => m.Id == id);
            CompetitionItemVM.SubCompetition = await _db.SubCompetition.Where(s => s.CompetitionId == CompetitionItemVM.CompetitionItem.CompetitionId).ToListAsync();

            if (CompetitionItemVM.Competition == null)
            {
                return NotFound();
            }
            return View(CompetitionItemVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CompetitionItemVM.CompetitionItem.SubCompetitionId = Convert.ToInt32(Request.Form["SubCompetitionId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(CompetitionItemVM);
            }

            _db.CompetitionItem.Add(CompetitionItemVM.CompetitionItem);
            await _db.SaveChangesAsync();

            //Work on the image saving section

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb = await _db.CompetitionItem.FindAsync(CompetitionItemVM.CompetitionItem.Id);

            if (files.Count > 0)
            {
                //files has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, CompetitionItemVM.CompetitionItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                menuItemFromDb.Image = @"\images\" + CompetitionItemVM.CompetitionItem.Id + extension;
            }
            else
            {
                //no file was uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultCompetitionImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + CompetitionItemVM.CompetitionItem.Id + ".png");
                menuItemFromDb.Image = @"\images\" + CompetitionItemVM.CompetitionItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}