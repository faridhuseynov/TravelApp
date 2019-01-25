using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Messages
{
    class MapReviewMessage
    {
        public ICollection<DestinationList> Destinations { get; set; } = new ObservableCollection<DestinationList>();
    }
}
