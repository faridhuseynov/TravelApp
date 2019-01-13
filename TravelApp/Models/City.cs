using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;

namespace TravelApp.Models
{
    public class City
    {
        public int Id { get; set; }

        public string CityName { get; set; }

        public string Country { get; set; }

        public string Currency { get; set; }

        public string ImagePath { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public IEnumerable<Trip> Trips { get; set; }
    }
}
