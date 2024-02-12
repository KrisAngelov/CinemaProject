using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }

        public int Hall { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public int SeatsCount { get; set; }

        public bool Taken { get; set; }

        [Required]
        public Showtime Showtime { get; set; }

        [ForeignKey("Showtime")]
        public string ShowtimeId { get; set; }

        public Ticket Ticket { get; set; }

        [ForeignKey("Ticket")]
        public string TicketId { get; set; }

        public Seat() 
        {
            Taken = false;
        }

        public Seat(int hall, int row, int column, int seatscount, Showtime showtime, Ticket ticket = null)
        {
            Hall = hall;
            Row = row;
            Column = column;
            SeatsCount = seatscount;
            Showtime = showtime;
            Ticket = ticket;
            Taken = false;
        }
    }
}
