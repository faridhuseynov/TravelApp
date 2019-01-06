using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    class Trip
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string TripName { get; set; }

        ////[Required]
        //public IEnumerable<City> Destinations { get; set; }


        //[Required]
        public DateTime Departure { get; set; } 

        //[Required]
        public DateTime Arrival { get; set; } 

        public int UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<TripTask> Tasks { get; set; }

        public IEnumerable<City> Destinations { get; set; }

    }
}

