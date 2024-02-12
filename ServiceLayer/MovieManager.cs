using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class MovieManager
    {
        private readonly MovieContext movieContext;

        public MovieManager(MovieContext movieContext)
        {
            this.movieContext = movieContext;
        }

        public async Task CreateAsync(Movie movie)
        {
            await movieContext.CreateAsync(movie);
        }

        public async Task<Movie> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await movieContext.ReadAsync(key, useNavigationalProperties, isReadOnly);
        }

        public async Task<ICollection<Movie>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            return await movieContext.ReadAllAsync(useNavigationalProperties, isReadOnly);
        }

        public async Task UpdateAsync(Movie item, bool useNavigationalProperties = false)
        {
            await movieContext.UpdateAsync(item, useNavigationalProperties);
        }

        public async Task DeleteAsync(int key)
        {
            await movieContext.DeleteAsync(key);
        }
    }
}
