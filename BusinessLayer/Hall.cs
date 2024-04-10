using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Hall
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int SeatsCount { get; set; }
        public ICollection<Showtime> Showtimes { get; set; }
        public ICollection<Seat> Seats { get; set; }

        public Hall()
        {
            Showtimes = new List<Showtime>();
            Seats = new List<Seat>();
        }

        public Hall(int number, int seatsCount)
        {
            Number = number;
            SeatsCount = seatsCount;
            Showtimes = new List<Showtime>();
            Seats = new List<Seat>();
        }
    }
}
