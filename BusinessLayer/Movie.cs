using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        public decimal Duration { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Director { get; set; }

        public decimal? Rating { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public Movie()
        {
            Showtimes = new List<Showtime>();
            Reviews = new List<Review>();
        }

        public Movie(string title, decimal duration, string description, string director, 
             decimal? rating = 0)
        {
            Title = title;
            Duration = duration;
            Description = description;
            Director = director;
            Rating = rating;
            Showtimes = new List<Showtime>();
            Reviews = new List<Review>();
        }
    }
}