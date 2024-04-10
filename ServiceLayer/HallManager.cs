using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class HallManager
    {
        private readonly HallContext hallContext;

        public HallManager(HallContext hallContext)
        {
            this.hallContext = hallContext;
        }

        public async Task CreateAsync(Hall hall)
        {
            await hallContext.CreateAsync(hall);
        }

        public async Task<Hall> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await hallContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Hall>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await hallContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Hall item, bool useNavigationalProperties = false)
        {
            await hallContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await hallContext.DeleteAsync(key);
        }
    }
}
