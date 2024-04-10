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

namespace MVC.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly ShowtimeManager showtimeManager;
        private readonly MovieManager movieManager;
        private readonly HallManager hallManager;

        public ShowtimesController(ShowtimeManager showtimeManager, MovieManager movieManager, HallManager hallManager)
        {
            this.showtimeManager = showtimeManager;
            this.movieManager = movieManager;
            this.hallManager = hallManager;
        }

        // GET: Showtimes
        public async Task<IActionResult> Index()
        {
            return View(await showtimeManager.ReadAllAsync(true, true));
        }

        // GET: Showtimes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = await showtimeManager.ReadAsync(id, true);

            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // GET: Showtimes/Create
        public async Task<IActionResult> Create()
        {
            await LoadNavigationalEntities();
            return View();
        }

        // POST: Showtimes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            await LoadNavigationalEntities();

            Movie movie = await movieManager.ReadAsync(int.Parse(formCollection["MovieId"]));
            Hall hall = await hallManager.ReadAsync(int.Parse(formCollection["HallId"]));

            DateTime startTime;
            // Remember to change the <input type="date"> otherwise the string won't be bound to the DateTime!
            bool hasStartDateTime = DateTime.TryParse(formCollection["StartTime"], out startTime);

            DateTime endTime;
            // Remember to change the <input type="date"> otherwise the string won't be bound to the DateTime!
            bool hasEndDateTime = DateTime.TryParse(formCollection["EndTime"], out endTime);

            Showtime showtime = new Showtime(movie, startTime,
                endTime, hall);

            if (hasStartDateTime)
            {
                showtime.StartTime = startTime;
            }
            if (hasEndDateTime)
            {
                showtime.EndTime = endTime;
            }

            if (ModelState.IsValid)
            {
                await showtimeManager.CreateAsync(showtime);
                return RedirectToAction(nameof(Index));
            }

            return View(showtime);
        }

        // GET: Showtimes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = await showtimeManager.ReadAsync(id);
            if (showtime == null)
            {
                return NotFound();
            }

            await LoadNavigationalEntities();
            return View(showtime);
        }

        // POST: Showtimes/Edit/5
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

            Movie movie = await movieManager.ReadAsync(int.Parse(formCollection["MovieId"]));
            Hall hall = await hallManager.ReadAsync(int.Parse(formCollection["HallId"]));

            DateTime startTime;
            // Remember to change the <input type="date"> otherwise the string won't be bound to the DateTime!
            bool hasStartDateTime = DateTime.TryParse(formCollection["StartTime"], out startTime);

            DateTime endTime;
            // Remember to change the <input type="date"> otherwise the string won't be bound to the DateTime!
            bool hasEndDateTime = DateTime.TryParse(formCollection["EndTime"], out endTime);

            Showtime showtime = new Showtime(movie, startTime,
                endTime, hall);

            if (hasStartDateTime)
            {
                showtime.StartTime = startTime;
            }
            if (hasEndDateTime)
            {
                showtime.EndTime = endTime;
            }

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

                    await showtimeManager.UpdateAsync(showtime, useNavigationalProperties);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ShowtimeExists(showtime.Id))
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

            return View(showtime);
        }

        // GET: Showtimes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showtime = await showtimeManager.ReadAsync(id);
            if (showtime == null)
            {
                return NotFound();
            }

            return View(showtime);
        }

        // POST: Showtimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await showtimeManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ShowtimeExists(int id)
        {
            return await showtimeManager.ReadAsync(id) is not null;
        }
        private async Task LoadNavigationalEntities()
        {
            ViewData["Movies"] = new SelectList(await movieManager.ReadAllAsync(), "Id", "Name");
            ViewData["Halls"] = new SelectList(await hallManager.ReadAllAsync(), "Id", "Number");
        }
    }
}
