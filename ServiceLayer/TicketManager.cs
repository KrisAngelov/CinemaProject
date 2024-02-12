using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class TicketManager
    {
        private readonly TicketContext ticketContext;

        public TicketManager(TicketContext ticketContext)
        {
            this.ticketContext = ticketContext;
        }

        public async Task CreateAsync(Ticket ticket)
        {
            await ticketContext.CreateAsync(ticket);
        }

        public async Task<Ticket> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await ticketContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Ticket>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await ticketContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Ticket item, bool useNavigationalProperties = false)
        {
            await ticketContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await ticketContext.DeleteAsync(key);
        }
    }
}
