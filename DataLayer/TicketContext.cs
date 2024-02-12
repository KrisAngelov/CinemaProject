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
    public class TicketContext : IDb<Ticket, int>
    {
        private readonly CinemaDbContext dbContext;

        public TicketContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Ticket item)
        {
            try
            {
                Showtime showtimeFromDb = await dbContext.Showtimes.FindAsync(item.Showtime);

                if (showtimeFromDb != null)
                {
                    item.Showtime = showtimeFromDb;
                }

                User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                if (userFromDb != null)
                {
                    item.User = userFromDb;
                }

                dbContext.Tickets.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Ticket> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Ticket> query = dbContext.Tickets;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Showtime).Include(a => a.User).Include(a => a.Seats);
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

        public async Task<ICollection<Ticket>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Ticket> query = dbContext.Tickets;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Showtime).Include(a => a.User).Include(a => a.Seats);
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

        public async Task UpdateAsync(Ticket item, bool useNavigationalProperties = false)
        {
            try
            {
                Ticket ticketFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (ticketFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(ticketFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
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
                    ticketFromDb.Seats = seats;

                    Showtime showtimeFromDb = await dbContext.Showtimes.FindAsync(item.ShowtimeId);

                    if (showtimeFromDb != null)
                    {
                        ticketFromDb.Showtime = showtimeFromDb;
                    }
                    else
                    {
                        ticketFromDb.Showtime = item.Showtime;
                    }

                    User userFromDb = await dbContext.Users.FindAsync(item.UserId);

                    if (userFromDb != null)
                    {
                        ticketFromDb.User = userFromDb;
                    }
                    else
                    {
                        ticketFromDb.User = item.User;
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
                Ticket ticketFromDb = await ReadAsync(key, false, false);

                if (ticketFromDb is null)
                {
                    throw new ArgumentException("Ticket with that Id does not exist!");
                }

                dbContext.Tickets.Remove(ticketFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
