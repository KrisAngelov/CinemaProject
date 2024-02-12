using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Movie Movie { get; set; } 

        [ForeignKey("Movie")]
        public string MovieId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime Endtime { get; set; }

        public ICollection<Seat> Seats { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public Showtime()
        {
            Seats = new List<Seat>();
            Tickets = new List<Ticket>();
        }

        public Showtime(Movie movie, DateTime startTime, DateTime endtime)
        {
            Movie = movie;
            StartTime = startTime;
            Endtime = endtime;
            Seats = new List<Seat>();
            Tickets = new List<Ticket>();
        }
    }
}
