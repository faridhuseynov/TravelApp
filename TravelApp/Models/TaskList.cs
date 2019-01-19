using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class TaskList
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public bool Status { get; set; } = false;
        public Trip Trip { get; set; }

    }
}
