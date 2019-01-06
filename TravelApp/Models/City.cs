using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
