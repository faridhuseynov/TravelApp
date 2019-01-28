using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TripName { get;set; }

        [Required]
        //public DateTime Departure { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime Departure { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        [Required]
        public DateTime Arrival { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<TaskList> TaskList { get; set; }
        public virtual ICollection<DestinationList> Destinations { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }


    }
}

