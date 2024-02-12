using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class MovieContext : IDb<Movie, int>
    {
        private readonly CinemaDbContext dbContext;

        public MovieContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Movie item)
        {
            try
            {
                dbContext.Movies.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Movie> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Movie> query = dbContext.Movies;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Reviews).Include(a => a.Showtimes);
                }

                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                return await query.FirstOrDefaultAsync(a => a.Id == key);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Movie>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Movie> query = dbContext.Movies;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Reviews).Include(a => a.Showtimes);
                }

                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateAsync(Movie item, bool useNavigationalProperties = false)
        {
            try
            {
                Movie movieFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (movieFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(movieFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    List<Review> reviews = new List<Review>(item.Reviews.Count);

                    foreach (var review in item.Reviews)
                    {
                        Review reviewFromDb = await dbContext.Reviews.FindAsync(review.Id);

                        if (reviewFromDb is null)
                        {
                            reviews.Add(review);
                        }
                        else
                        {
                            reviews.Add(reviewFromDb);
                        }
                    }
                    movieFromDb.Reviews = reviews;

                    List<Showtime> showtimes = new List<Showtime>(item.Showtimes.Count);

                    foreach (var showtime in item.Showtimes)
                    {
                        Showtime showtimeFromDb = await dbContext.Showtimes.FindAsync(showtime.Id);

                        if (showtimeFromDb is null)
                        {
                            showtimes.Add(showtime);
                        }
                        else
                        {
                            showtimes.Add(showtimeFromDb);
                        }
                    }
                    movieFromDb.Showtimes = showtimes;
                }

                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Movie movieFromDb = await ReadAsync(key, false, false);

                if (movieFromDb is null)
                {
                    throw new ArgumentException("Movie with that Id does not exist!");
                }

                dbContext.Movies.Remove(movieFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
