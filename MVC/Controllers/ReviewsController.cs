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
    public class ReviewsController : Controller
    {
        private readonly ReviewManager reviewManager;
        private readonly IdentityManager identityManager;
        private readonly MovieManager movieManager;

        public ReviewsController(ReviewManager reviewManager, IdentityManager identityManager, MovieManager movieManager)
        {
            this.reviewManager = reviewManager;
            this.identityManager = identityManager;
            this.movieManager = movieManager;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            return View(await reviewManager.ReadAllAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await reviewManager.ReadAsync((int)id, true, true);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public async Task<IActionResult> Create()
        {
            await LoadNavigationalEntities();
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection)
        {
            await LoadNavigationalEntities();

            decimal rating = decimal.Parse(formCollection["Rating"]);

            Review review = new Review(formCollection["Comment"], rating, null, null);

            if (!string.IsNullOrEmpty(formCollection["UserId"]))
            {
                User user = await identityManager.ReadUserAsync(formCollection["UserId"]);
                review.User = user;
                review.UserId = formCollection["UserId"];
            }

            if (!string.IsNullOrEmpty(formCollection["MovieId"]))
            {
                Movie movie = await movieManager.ReadAsync(int.Parse(formCollection["MovieId"]));
                review.Movie = movie;
                review.MovieId = int.Parse(formCollection["MovieId"]);
            }

            if (ModelState.IsValid)
            {
                await reviewManager.CreateAsync(review);
                return RedirectToAction(nameof(Index));
            }

            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await reviewManager.ReadAsync((int)id);
            if (review == null)
            {
                return NotFound();
            }
            await LoadNavigationalEntities();
            return View(review);
        }

        // POST: Reviews/Edit/5
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

            decimal rating = decimal.Parse(formCollection["Rating"]);

            Review review = new Review(formCollection["Comment"], rating, null, null);
            review.Id = id;

            if (!string.IsNullOrEmpty(formCollection["UserId"]))
            {
                User user = await identityManager.ReadUserAsync(formCollection["UserId"]);
                review.User = user;
                review.UserId = formCollection["UserId"];
            }

            if (!string.IsNullOrEmpty(formCollection["MovieId"]))
            {
                Movie movie = await movieManager.ReadAsync(int.Parse(formCollection["MovieId"]));
                review.Movie = movie;
                review.MovieId = int.Parse(formCollection["MovieId"]);
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

                    await reviewManager.UpdateAsync(review, useNavigationalProperties);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ReviewExists(review.Id))
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

            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await reviewManager.ReadAsync((int)id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await reviewManager.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ReviewExists(int id)
        {
            return await reviewManager.ReadAsync(id) is not null;
        }

        private async Task LoadNavigationalEntities()
        {
            IEnumerable<User> users = await identityManager.ReadAllUsersAsync();
            ViewData["User"] = new SelectList(users, "Id", "Id");
            ICollection<Movie> movies = await movieManager.ReadAllAsync();
            ViewData["Movie"] = new SelectList(movies, "Id", "Id");
        }
    }
}
