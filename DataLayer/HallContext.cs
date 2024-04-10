using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class HallContext : IDb<Hall, int>
    {
        private readonly CinemaDbContext dbContext;

        public HallContext(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(Hall item)
        {
            try
            {
                dbContext.Halls.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Hall> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hall> query = dbContext.Halls;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Seats).Include(a => a.Showtimes);
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

        public async Task<ICollection<Hall>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Hall> query = dbContext.Halls;

                if (useNavigationalProperties)
                {
                    query = query.Include(a => a.Showtimes).Include(a => a.Seats);
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

        public async Task UpdateAsync(Hall item, bool useNavigationalProperties = false)
        {
            try
            {
                Hall hallFromDb = await ReadAsync(item.Id, useNavigationalProperties, false);

                if (hallFromDb == null) { await CreateAsync(item); }

                dbContext.Entry(hallFromDb).CurrentValues.SetValues(item);

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
                    hallFromDb.Seats = seats;
                    
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
                    hallFromDb.Showtimes = showtimes;
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
                Hall hallFromDb = await ReadAsync(key, false, false);

                if (hallFromDb is null)
                {
                    throw new ArgumentException("Hall with that Id does not exist!");
                }

                dbContext.Halls.Remove(hallFromDb);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
