using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required]
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }       
        [Required]
        public Showtime Showtime { get; set; }
        [ForeignKey("Showtime")]
        public int ShowtimeId { get; set; }
        public ICollection<Seat> Seats { get; set; }

        public Ticket()
        {
            Seats = new List<Seat>();
        }

        public Ticket(User user, Showtime showtime, bool paid, decimal price = 0)
        {
            User = user;
            Showtime = showtime;
            Paid = false;
            Price = price;
            Seats = new List<Seat>();
        }
    }
}
