using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    class TripTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public bool Status { get; set; } = false;
    }
}
