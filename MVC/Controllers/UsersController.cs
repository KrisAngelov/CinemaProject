﻿using BusinessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceLayer;

namespace MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IdentityManager userManager;
        private readonly TicketManager ticketManager;
        private readonly SeatManager seatManager;
        private readonly MovieManager movieManager;
        private readonly ShowtimeManager showtimeManager;

        public UsersController(IdentityManager manager, TicketManager ticketManager, SeatManager seatManager,
            MovieManager movieManager, ShowtimeManager showtimeManager)
        {
            userManager = manager;
            this.ticketManager = ticketManager; 
            this.seatManager = seatManager;
            this.movieManager = movieManager;
            this.showtimeManager = showtimeManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await userManager.ReadAllUsersAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            User user = await userManager.ReadUserAsync(id, true);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: Sports/Create
        public IActionResult Create()
        {
            LoadNavigationalEntities();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, UserName, Password, Email, FirstName, LastName, Age, Role")] User user)
        {
            if (ModelState.IsValid)
            {
                await userManager.CreateUserAsync(user.UserName, user.Password, user.Email, user.FirstName, user.LastName, user.Age, user.Role);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.ReadUserAsync(id, true);
            LoadNavigationalEntities();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, UserName, Password, Email, FirstName, LastName, Age, Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await userManager.UpdateUserAsync(user.Id, user.UserName, user.FirstName, user.LastName, user.Age);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.ReadUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            User user = await userManager.ReadUserAsync(id);
            if (user != null)
            {
                await userManager.DeleteUserByNameAsync(user.UserName);
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task<bool> USerExists(string id)
        {
            return await userManager.ReadUserAsync(id) is not null;
        }
        private void LoadNavigationalEntities()
        {
            ViewData["Role"] = new SelectList(Enum.GetValues(typeof(Role)));
        }

        public async Task<IActionResult> Statistics()
        {
            ViewData["UserCount"] = (await userManager.ReadAllUsersAsync()).Count();
            ViewData["TicketCount"] = (await ticketManager.ReadAllAsync()).Count();
            ViewData["TakenSeats"] = (await seatManager.ReadAllAsync()).Where(s => s.Availability == SeatAvailability.Taken).Count();
            ViewData["MovieCount"] = (await movieManager.ReadAllAsync()).Count();
            ViewData["ShowtimeCount"] = (await showtimeManager.ReadAllAsync()).Count();
            return View();
        }
    }
}