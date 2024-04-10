﻿using System;
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

namespace MVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketManager ticketManager;
        private readonly ShowtimeManager showtimeManager;
        private readonly IdentityManager userManager;

        public TicketsController(TicketManager ticketManager, ShowtimeManager showtimeManager, IdentityManager userManager)
        {
            this.ticketManager = ticketManager;
            this.showtimeManager = showtimeManager;
            this.userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            return View(await ticketManager.ReadAllAsync(true, true));
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

            Showtime showtime = await showtimeManager.ReadAsync(int.Parse(formCollection["ShowtimeId"]));
            User user = await userManager.ReadUserAsync(formCollection["UserId"]);

            decimal price = decimal.Parse(formCollection["Price"]);

            Ticket ticket = new Ticket(user, showtime, price);

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

            Showtime showtime = await showtimeManager.ReadAsync(int.Parse(formCollection["ShowtimeId"]));
            User user = await userManager.ReadUserAsync(formCollection["UserId"]);

            decimal price = decimal.Parse(formCollection["Price"]);

            Ticket ticket = new Ticket(user, showtime, price);

            if (ModelState.IsValid)
            {
                try
                {
                    bool useNavigationalProperties = false;

                    // If the checkbox is not clicked, the browser does not send even our 'false' value!
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
            ViewData["Showtimes"] = new SelectList(await showtimeManager.ReadAllAsync(), "Id", "Id");
            ViewData["Users"] = new SelectList(await userManager.ReadAllUsersAsync(), "Id", "FirstName");
        }
    }
}