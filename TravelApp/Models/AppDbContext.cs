using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppConnection")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripTask> Tasks { get; set; }
        public DbSet <City> Destionations { get; set; }
        public int LoggedInUser { get; set; }
    }
}
