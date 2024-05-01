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

namespace MVC.Controllers
{
    public class SeatsController : Controller
    {
        private readonly SeatManager seatManager;
        private readonly HallManager hallManager;
        private readonly TicketManager ticketManager;

        public SeatsController(SeatManager seatManager, HallManager hallManager, TicketManager ticketManager)
        {
            this.seatManager = seatManager;
            this.hallManager = hallManager;
            this.ticketManager = ticketManager;
        }

        // GET: Seats
        public async Task<IActionResult> Index()
        {
            await LoadNavigationalEntities();
            return View(await seatManager.ReadAllAsync(true));
        }

        // GET: Seats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await seatManager.ReadAsync((int)id, true, true);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // GET: Seats/Create
        public async Task<IActionResult> Create()
        {
            await LoadNavigationalEntities();
            return View();
        }

        // POST: Seats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            await LoadNavigationalEntities();

            Hall hall = await hallManager.ReadAsync(int.Parse(formCollection["HallId"]));
            Seat seat = new Seat(int.Parse(formCollection["Row"]), int.Parse(formCollection["Column"]),
                (SeatAvailability)Enum.Parse(typeof(SeatAvailability), formCollection["Availability"]), hall, null);

            if (!string.IsNullOrEmpty(formCollection["HallId"]))
            {
                Hall hall1 = await hallManager.ReadAsync(int.Parse(formCollection["HallId"]));
                seat.Hall = hall1;
                seat.HallId = int.Parse(formCollection["HallId"]);
            }

            if (!string.IsNullOrEmpty(formCollection["TicketId"]))
            {
                Ticket ticket = await ticketManager.ReadAsync(int.Parse(formCollection["TicketId"]));
                seat.Ticket = ticket;
                seat.TicketId = int.Parse(formCollection["TicketId"]);
            }

            if (ModelState.IsValid)
            {
                await seatManager.CreateAsync(seat);
                return RedirectToAction(nameof(Index));
            }

            return View(seat);
        }

        // GET: Seats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await seatManager.ReadAsync((int)id);
            if (seat == null)
            {
                return NotFound();
            }

            await LoadNavigationalEntities();
            return View(seat);
        }

        // POST: Seats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection formCollection)
        {
            if (id != int.Parse(formCollection["Id"]))
            {
                return NotFound();
            }

            await LoadNavigationalEntities();

            SeatAvailability availability = (SeatAvailability)Enum.Parse(typeof(SeatAvailability), formCollection["Availability"]);
            Seat seat = new Seat(int.Parse(formCollection["Row"]), int.Parse(formCollection["Column"]), availability, null, null);
            seat.Id = id;

            if (!string.IsNullOrEmpty(formCollection["HallId"]))
            {
                Hall hall = await hallManager.ReadAsync(int.Parse(formCollection["HallId"]));
                seat.Hall = hall;
                seat.HallId = int.Parse(formCollection["HallId"]);
            }

            if (!string.IsNullOrEmpty(formCollection["TicketId"]))
            {
                Ticket ticket = await ticketManager.ReadAsync(int.Parse(formCollection["TicketId"]));
                seat.Ticket = ticket;
                seat.TicketId = int.Parse(formCollection["TicketId"]);
            }

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

                    await seatManager.UpdateAsync(seat, useNavigationalProperties);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SeatExists(seat.Id))
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
            return View(seat);
        }

        // GET: Seats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = await seatManager.ReadAsync((int)id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        // POST: Seats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await seatManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SeatExists(int id)
        {
            return await seatManager.ReadAsync(id) is not null;
        }

        private async Task LoadNavigationalEntities()
        {
            ICollection<Hall> halls = await hallManager.ReadAllAsync();
            ViewData["Halls"] = new SelectList(halls, "Id", "Number");
            ICollection<Ticket> tickets = await ticketManager.ReadAllAsync();
            ViewData["Tickets"] = new SelectList(tickets, "Id", "Id");
            ViewData["Availability"] = new SelectList(Enum.GetValues(typeof(SeatAvailability)));
        }
    }
}
