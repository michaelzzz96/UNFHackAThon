﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models;
using UNFHackAThon.Utility;

namespace UNFHackAThon.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManageUser)]
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


        //POST -CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Competition competition)
        {
            if(ModelState.IsValid)
            {
                //if valid
                _db.Competition.Add(competition);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");

            }
            return View(competition);
        }

        //GET - EDIT
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var competition = await _db.Competition.FindAsync(id);
            if(competition == null)
            {
                return NotFound();
            }
            return View(competition);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Competition competition)
        {
            if (ModelState.IsValid)
            {
                _db.Update(competition);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        //GET - DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var competition = await _db.Competition.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var competition = await _db.Competition.FindAsync(id);

            if (competition == null)
            {
                return View();
            }
            _db.Competition.Remove(competition);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET - DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _db.Competition.FindAsync(id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }


    }
}