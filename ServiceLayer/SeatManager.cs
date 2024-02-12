using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class SeatManager
    {
        private readonly SeatContext seatContext;

        public SeatManager(SeatContext seatContext)
        {
            this.seatContext = seatContext;
        }

        public async Task CreateAsync(Seat seat)
        {
            await seatContext.CreateAsync(seat);
        }

        public async Task<Seat> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await seatContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Seat>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await seatContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Seat item, bool useNavigationalProperties = false)
        {
            await seatContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await seatContext.DeleteAsync(key);
        }
    }
}
