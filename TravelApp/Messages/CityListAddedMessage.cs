using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Messages
{
    class CityListAddedMessage
    {
        public ObservableCollection<City> NewCityList { get; set; } = new ObservableCollection<City>();
    }
}
