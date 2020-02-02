﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Data;
using UNFHackAThon.Models;
using UNFHackAThon.Models.ViewModels;
using UNFHackAThon.Utility;

namespace UNFHackAThon.Areas.Participants.Controllers
{
    [Area("Participants")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _db.CompetitionCart.Where(c => c.ApplicationUserId == claim.Value);
            if(cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach(var list in detailCart.listCart)
            {
                list.CompetitionItem = await _db.CompetitionItem.FirstOrDefaultAsync(m => m.Id == list.CompetitionItemId);
                detailCart.OrderHeader = detailCart.OrderHeader = detailCart.OrderHeader;
                list.CompetitionItem.Description = SD.ConvertToRawHtml(list.CompetitionItem.Description);
                if(list.CompetitionItem.Description.Length > 100)
                {
                    list.CompetitionItem.Description = list.CompetitionItem.Description.Substring(0, 99) + "....";
                }
            }
            return View(detailCart);
        }

        public async Task<IActionResult> Summary()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            var cart = _db.CompetitionCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.CompetitionItem = await _db.CompetitionItem.FirstOrDefaultAsync(m => m.Id == list.CompetitionItemId);
                detailCart.OrderHeader = detailCart.OrderHeader = detailCart.OrderHeader;
               
            }
            detailCart.OrderHeader.PickupName = applicationUser.Name;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;




            return View(detailCart);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.CompetitionCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.CompetitionCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.CompetitionCart.Remove(cart);
                await _db.SaveChangesAsync();

                var cnt = _db.CompetitionCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssCompetitionCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.CompetitionCart.FirstOrDefaultAsync(c => c.Id == cartId);

            _db.CompetitionCart.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.CompetitionCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ssCompetitionCartCount, cnt);


            return RedirectToAction(nameof(Index));
        }


    }
}