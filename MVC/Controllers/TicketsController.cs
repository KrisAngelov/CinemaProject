using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using ServiceLayer;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Stripe.Checkout;

namespace MVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketManager ticketManager;
        private readonly ShowtimeManager showtimeManager;
        private readonly IdentityManager identityManager;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IdentityContext identityContext;

        public TicketsController(TicketManager ticketManager, ShowtimeManager showtimeManager, 
            IdentityManager identityManager, SignInManager<User> signInManager, UserManager<User> userManager, 
            IdentityContext identityContext)
        {
            this.ticketManager = ticketManager;
            this.showtimeManager = showtimeManager;
            this.identityManager = identityManager;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.identityContext = identityContext;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            if(identityContext.IsUserAdmin(await userManager.FindByNameAsync(User.Identity.Name)))
            {
                return View(await ticketManager.ReadAllAsync(true, true));
            }
            else
            {
                string UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;
                return View((await ticketManager.ReadAllAsync(true, true)).Where(t => t.UserId == UserId));
            }
            
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await ticketManager.ReadAsync(id, true);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public async Task<IActionResult> Create()
        {

            await LoadNavigationalEntities();
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            await LoadNavigationalEntities();

            Showtime showtime = await showtimeManager.ReadAsync(int.Parse(formCollection["ShowtimeId"]), false, false);
            User user = await identityManager.ReadUserAsync(formCollection["UserId"]);

            decimal price = decimal.Parse(formCollection["Price"]);

            Ticket ticket = new Ticket(user, showtime, price);
            ticket.UserId = (await userManager.FindByNameAsync(User.Identity.Name)).Id;

            var additionalInfo1 = formCollection["additionalInfo1"];
            var additionalInfo2 = formCollection["additionalInfo2"];

            if (ModelState.IsValid)
            {
                await ticketManager.CreateAsync(ticket);
                return RedirectToAction(nameof(Index));
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await ticketManager.ReadAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            await LoadNavigationalEntities();
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection formCollection)
        {
            if (id != formCollection["Id"])
            {
                return NotFound();
            }
            await LoadNavigationalEntities();
            Showtime showtime = await showtimeManager.ReadAsync(int.Parse(formCollection["ShowtimeId"]), false, false);
            User user = await identityManager.ReadUserAsync(formCollection["UserId"]);

            decimal price = decimal.Parse(formCollection["Price"]);

            Ticket ticket = new Ticket(user, showtime, price);

            if (ModelState.IsValid)
            {
                try
                {
                    bool useNavigationalProperties = false;
                    string checkboxValue = formCollection["useNavigationalProperties"];
                    if (!string.IsNullOrEmpty(checkboxValue))
                    {
                        useNavigationalProperties = true;
                    }

                    await ticketManager.UpdateAsync(ticket, useNavigationalProperties);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            await LoadNavigationalEntities();

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await ticketManager.ReadAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await ticketManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TicketExists(int id)
        {
            return await ticketManager.ReadAsync(id) is not null;
        }

        private async Task LoadNavigationalEntities()
        {
            ICollection<Showtime> showtimes = await showtimeManager.ReadAllAsync();
            ViewData["Showtimes"] = new SelectList(showtimes, "Id", "StartTime");
        }

        public IActionResult CheckOut(int id)
        {
            var ticket = ticketManager.ReadAsync(id);

            var options = new SessionCreateOptions
            {
                SuccessUrl = $"https://localhost:7297/Tickets/OrderConfirmation",
                CancelUrl = $"https://localhost:7297/Tickets/Fail",
                LineItems = new List<SessionLineItemOptions>
                {
                    new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(ticket.Result.Price)*100,
                            Currency = "bgn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = ticket.Result.ShowtimeId.ToString(),
                            }
                        },                  
                        Quantity = 1,
                    },
                },
                Mode = "payment"
            };

            var service = new SessionService();
            var session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            var session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();

                return View("Success");
            }
            return View("Fail");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Fail()
        {
            return View();
        }
    }
}
