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
    public class SeatContext : IDb<Seat, int>
    {
        private readonly CinemaDbContext dbContext;

        public SeatContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Seat item)
        {
            try
            {
                Showtime showtimeFromDb = await dbContext.Showtimes.FindAsync(item.ShowtimeId);

                if (showtimeFromDb != null)
                {
                    item.Showtime = showtimeFromDb;
                }

                Ticket ticketFromDb = await dbContext.Tickets.FindAsync(item.TicketId);

                if (ticketFromDb != null)
                {
                    item.Ticket = ticketFromDb;
                }

                dbContext.Seats.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Seat> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Seat> query = dbContext.Seats;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Showtime).Include(a => a.Ticket);
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

        public async Task<ICollection<Seat>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Seat> query = dbContext.Seats;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Showtime).Include(a => a.Showtime);
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

        public async Task UpdateAsync(Seat item, bool useNavigationalProperties = false)
        {
            try
            {
                Seat seatFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (seatFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(seatFromDb).CurrentValues.SetValues(item);

                if (useNavigationalProperties)
                {
                    Ticket ticketFromDb = await dbContext.Tickets.FindAsync(item.TicketId);

                    if (ticketFromDb != null)
                    {
                        seatFromDb.Ticket = ticketFromDb;
                    }
                    else
                    {
                        seatFromDb.Ticket = item.Ticket;
                    }

                    Showtime showtimeFromDb = await dbContext.Showtimes.FindAsync(item.ShowtimeId);

                    if (showtimeFromDb != null)
                    {
                        seatFromDb.Showtime = showtimeFromDb;
                    }
                    else
                    {
                        seatFromDb.Showtime = item.Showtime;
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
                Seat seatFromDb = await ReadAsync(key, false, false);

                if (seatFromDb is null)
                {
                    throw new ArgumentException("Seat with that Id does not exist!");
                }

                dbContext.Seats.Remove(seatFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
