using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ReviewManager
    {
        private readonly ReviewContext reviewContext;

        public ReviewManager(ReviewContext reviewContext)
        {
            this.reviewContext = reviewContext;
        }

        public async Task CreateAsync(Review review)
        {
            await reviewContext.CreateAsync(review);
        }

        public async Task<Review> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await reviewContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Review>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await reviewContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Review item, bool useNavigationalProperties = false)
        {
            await reviewContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await reviewContext.DeleteAsync(key);
        }
    }
}
