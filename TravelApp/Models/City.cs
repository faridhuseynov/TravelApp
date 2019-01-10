using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace TravelApp.Models
{
    class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }

        public Location Coordinates { get; set; } = new Location();
        //public string Longitude { get; set; }
        //public string Lattitude { get; set; }
        public int TripId { get; set; }
        public Trip Trip { get; set; }
    }
}
