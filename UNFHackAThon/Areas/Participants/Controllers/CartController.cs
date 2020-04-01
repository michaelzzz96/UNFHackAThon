using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
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
        private readonly IEmailSender _emailSender;


        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public CartController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
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
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.CompetitionItem = await _db.CompetitionItem.FirstOrDefaultAsync(m => m.Id == list.CompetitionItemId);
                detailCart.OrderHeader = detailCart.OrderHeader = detailCart.OrderHeader;
                list.CompetitionItem.Description = SD.ConvertToRawHtml(list.CompetitionItem.Description);
                if (list.CompetitionItem.Description.Length > 100)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            detailCart.listCart = await _db.CompetitionCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();

            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailCart.OrderHeader);
            await _db.SaveChangesAsync();

            foreach (var item in detailCart.listCart)
            {
                item.CompetitionItem = await _db.CompetitionItem.FirstOrDefaultAsync(m => m.Id == item.CompetitionItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    CompetitionItemId = item.CompetitionItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Name = item.CompetitionItem.Name,
                    Count = item.Count

                };
                _db.OrderDetails.Add(orderDetails);

            }

            _db.CompetitionCart.RemoveRange(detailCart.listCart);
            HttpContext.Session.SetInt32(SD.ssCompetitionCartCount, 0);
            await _db.SaveChangesAsync();

            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email, "Competition Joined " + detailCart.OrderHeader.Id.ToString(), "Competition Joined successfully.");

            await _db.SaveChangesAsync();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Confirm", "Order", new { id = detailCart.OrderHeader.Id });


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

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsViewModel> orderList = new List<OrderDetailsViewModel>();

            List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.UserId == claim.Value).ToListAsync();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderList.Add(individual);
            }

            return View(orderList);
        }


    }
}