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
        public int MovieId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public Hall Hall { get; set; }
        [ForeignKey("Hall")]
        public int HallId { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

        public Showtime()
        {
            Tickets = new List<Ticket>();
        }

        public Showtime(Movie movie, DateTime startTime, DateTime endtime,Hall hall)
        {
            Movie = movie;
            StartTime = startTime;
            EndTime = endtime;
            Hall = hall;
            Tickets = new List<Ticket>();
        }
    }
}
