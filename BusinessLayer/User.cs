using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

        public int Age { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public User() 
        {
            Reviews = new List<Review>();
            Tickets = new List<Ticket>();
        }

        public User(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Reviews = new List<Review>();
            Tickets = new List<Ticket>();
        }
    }
}
