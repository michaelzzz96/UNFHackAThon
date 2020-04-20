using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models;
using UNFHackAThon.Models.ViewModels;
using UNFHackAThon.Utility;

namespace UNFHackAThon.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManageUser)]

    [Area("Admin")]

    public class SubCompetitionController : Controller
    {
        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }


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

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCompetitionAndCompetitionViewModel model)
        {
            if(ModelState.IsValid)
            {
                var doesSubCompetitionExists = _db.SubCompetition.Include(s => s.Competition).Where(s => s.Name == model.SubCompetition.Name && s.Competition.Id == model.SubCompetition.CompetitionId);

                    if(doesSubCompetitionExists.Count() > 0)
                    {
                    //Error 
                    StatusMessage = "Error : Sub Competition exists under " + doesSubCompetitionExists.First().Competition.Name + " Competition. Please use another name. ";
                    }
                else
                {
                    _db.SubCompetition.Add(model.SubCompetition);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCompetitionAndCompetitionViewModel modelVM = new SubCompetitionAndCompetitionViewModel()
            {
                CompetitionList = await _db.Competition.ToListAsync(),
                SubCompetition = model.SubCompetition,
                SubCompetitionList = await _db.SubCompetition.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        [ActionName("GetSubCompetition")]
        public async Task<IActionResult> GetSubCompetition(int id)
        {
            List<SubCompetition> subCompetitions = new List<SubCompetition>();
            subCompetitions = await (from SubCompetition in _db.SubCompetition
                               where SubCompetition.CompetitionId == id
                               select SubCompetition).ToListAsync();
            return Json(new SelectList(subCompetitions, "Id", "Name"));
        }


        //GET - EDIT
        public async Task<IActionResult> Edit(int?  id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var subCompetition = await _db.SubCompetition.SingleOrDefaultAsync(m => m.Id == id);

            if (subCompetition == null)
            {
                return NotFound();
            }

            SubCompetitionAndCompetitionViewModel model = new SubCompetitionAndCompetitionViewModel()
            {
                CompetitionList = await _db.Competition.ToListAsync(),
                SubCompetition = subCompetition,
                SubCompetitionList = await _db.SubCompetition.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCompetitionAndCompetitionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCompetitionExists = _db.SubCompetition.Include(s => s.Competition).Where(s => s.Name == model.SubCompetition.Name && s.Competition.Id == model.SubCompetition.CompetitionId);

                if (doesSubCompetitionExists.Count() > 0)
                {
                    //Error 
                    StatusMessage = "Error : Competition Type exists under " + doesSubCompetitionExists.First().Competition.Name + " Competition. Please use another name. ";
                }
                else
                {
                    var subCompFromDb = await _db.SubCompetition.FindAsync(model.SubCompetition.Id);
                    subCompFromDb.Name = model.SubCompetition.Name;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCompetitionAndCompetitionViewModel modelVM = new SubCompetitionAndCompetitionViewModel()
            {
                CompetitionList = await _db.Competition.ToListAsync(),
                SubCompetition = model.SubCompetition,
                SubCompetitionList = await _db.SubCompetition.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            //modelVM.SubCompetition.Id = id;
            return View(modelVM);
        }

        //GET Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCompetition = await _db.SubCompetition.Include(s => s.Competition).SingleOrDefaultAsync(m => m.Id == id);
            if (subCompetition == null)
            {
                return NotFound();
            }

            return View(subCompetition);
        }

        //GET Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _db.SubCompetition.Include(s => s.Competition).SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        //POST Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _db.SubCompetition.SingleOrDefaultAsync(m => m.Id == id);
            _db.SubCompetition.Remove(subCategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


    }
}