using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ShowtimeManager
    {
        private readonly ShowtimeContext showtimeContext;

        public ShowtimeManager(ShowtimeContext showtimeContext)
        {
            this.showtimeContext = showtimeContext;
        }

        public async Task CreateAsync(Showtime showtime)
        {
            await showtimeContext.CreateAsync(showtime);
        }

        public async Task<Showtime> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await showtimeContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Showtime>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await showtimeContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Showtime item, bool useNavigationalProperties = false)
        {
            await showtimeContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await showtimeContext.DeleteAsync(key);
        }
    }
}
