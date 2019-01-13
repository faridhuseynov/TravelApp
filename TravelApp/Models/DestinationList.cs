using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class DestinationList
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
