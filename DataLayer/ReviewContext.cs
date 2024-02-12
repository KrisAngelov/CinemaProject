using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class ReviewContext : IDb<Review, int>
    {
        private readonly CinemaDbContext dbContext;

        public ReviewContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Review item)
        {
            try
            {
                User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                if (userFromDb != null)
                {
                    item.User = userFromDb;
                }

                Movie movieFromDb = await dbContext.Movies.FindAsync(item.MovieId);

                if (movieFromDb != null)
                {
                    item.Movie = movieFromDb;
                }

                dbContext.Reviews.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Review> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Review> query = dbContext.Reviews;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.User).Include(a => a.Movie);
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

        public async Task<ICollection<Review>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Review> query = dbContext.Reviews;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.User).Include(a => a.Movie);
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

        public async Task UpdateAsync(Review item, bool useNavigationalProperties = false)
        {
            try
            {
                Review reviewFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (reviewFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(reviewFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                    if (userFromDb != null)
                    {
                        reviewFromDb.User = userFromDb;
                    }
                    else
                    {
                        reviewFromDb.User = item.User;
                    }

                    Movie movieFromDb = await dbContext.Movies.FindAsync(item.MovieId);

                    if (movieFromDb != null)
                    {
                        reviewFromDb.Movie = movieFromDb;
                    }
                    else
                    {
                        reviewFromDb.Movie = item.Movie;
                    }
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
                Review reviewFromDb = await ReadAsync(key, false, false);

                if (reviewFromDb is null)
                {
                    throw new ArgumentException("Review with that Id does not exist!");
                }

                dbContext.Reviews.Remove(reviewFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
