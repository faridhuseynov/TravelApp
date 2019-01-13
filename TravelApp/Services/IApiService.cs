using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Services
{
    interface IApiService
    {
        City GetCity(string CityName);
    }
}
