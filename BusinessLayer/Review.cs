using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal Rating { get; set; }

        [Required]
        public User User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public Movie Movie { get; set; }

        [ForeignKey("Movie")]
        public string MovieId { get; set; }

        public Review()
        {

        }

        public Review(string description, decimal rating, User user, Movie movie)
        {
            Description = description;
            Rating = rating;
            User = user;
            Movie = movie;
        }
    }
}
