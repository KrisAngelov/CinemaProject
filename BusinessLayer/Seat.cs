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

        public int Row { get; set; }

        public int Column { get; set; }

        public SeatAvailability Availability { get; set; }

        public Hall Hall { get; set; }

        [ForeignKey("Hall")]
        public int HallId { get; set; }

        public Ticket Ticket { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        public Seat() 
        {
        }

        public Seat(int row, int column, SeatAvailability availability, Hall hall = null, Ticket ticket = null)
        {
            Hall = hall;
            Row = row;
            Column = column;
            Availability = availability;
            Ticket = ticket;
        }
    }
}
