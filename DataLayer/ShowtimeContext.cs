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
    public class ShowtimeContext : IDb<Showtime, int>
    {
        private readonly CinemaDbContext dbContext;

        public ShowtimeContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Showtime item)
        {
            try
            {
                Movie movieFromDb = await dbContext.Movies.FindAsync(item.MovieId);

                if (movieFromDb != null)
                {
                    item.Movie = movieFromDb;
                }

                dbContext.Showtimes.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Showtime> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Showtime> query = dbContext.Showtimes;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Movie).Include(a => a.Tickets).Include(a => a.Seats);
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

        public async Task<ICollection<Showtime>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Showtime> query = dbContext.Showtimes;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Movie).Include(a => a.Tickets).Include(a => a.Seats);
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

        public async Task UpdateAsync(Showtime item, bool useNavigationalProperties = false)
        {
            try
            {
                Showtime showtimeFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (showtimeFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(showtimeFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    Movie movieFromDb = await dbContext.Movies.FindAsync(item.MovieId);

                    if (movieFromDb != null)
                    {
                        showtimeFromDb.Movie = movieFromDb;
                    }
                    else
                    {
                        showtimeFromDb.Movie = item.Movie;
                    }

                    List<Ticket> tickets = new List<Ticket>(item.Tickets.Count);

                    foreach (var ticket in item.Tickets)
                    {
                        Ticket ticketFromDb = await dbContext.Tickets.FindAsync(ticket.Id);

                        if (ticketFromDb is null)
                        {
                            tickets.Add(ticket);
                        }
                        else
                        {
                            tickets.Add(ticketFromDb);
                        }
                    }
                    showtimeFromDb.Tickets = tickets;

                    List<Seat> seats = new List<Seat>(item.Seats.Count);

                    foreach (var seat in item.Seats)
                    {
                        Seat seatFromDb = await dbContext.Seats.FindAsync(seat.Id);

                        if (seatFromDb is null)
                        {
                            seats.Add(seat);
                        }
                        else
                        {
                            seats.Add(seatFromDb);
                        }
                    }
                    showtimeFromDb.Seats = seats;
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
                Showtime showtimeFromDb = await ReadAsync(key, false, false);

                if (showtimeFromDb is null)
                {
                    throw new ArgumentException("Showtime with that Id does not exist!");
                }

                dbContext.Showtimes.Remove(showtimeFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
