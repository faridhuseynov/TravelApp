using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TravelApp.Models;

namespace TravelApp.Services
{
    class ApiService : IApiService
    {
        public City GetCity(string CityName)
        {
            var NewCity = new City();
            int count;
            string request_uri= $"https://api.teleport.org/api/cities/?search={CityName}&limit=1";
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    var request = webClient.DownloadString(request_uri);
                    var result = JsonConvert.DeserializeObject(request) as JObject;
                    request_uri=result["_embedded"]["city:search-results"][0]["_links"]["city:item"]["href"].ToString();
                    count = int.Parse(result["count"].ToString());
                    if (count==0)
                    {
                        MessageBox.Show("City information not found, please check the city name");
                        return null;
                    }
                    request = webClient.DownloadString(request_uri);
                    result = JsonConvert.DeserializeObject(request) as JObject;
                    NewCity.Country = result["_links"]["city:country"]["name"].ToString();
                    NewCity.CityName = result["name"].ToString();
                    NewCity.Latitude = result["location"]["latlon"]["latitude"].ToString();
                    NewCity.Longitude = result["location"]["latlon"]["longitude"].ToString();

                    var image_uri = result["_links"]["city:urban_area"]["href"].ToString();

                    request_uri = result["_links"]["city:country"]["href"].ToString();
                    request = webClient.DownloadString(request_uri);
                    result = JsonConvert.DeserializeObject(request) as JObject;
                    NewCity.Currency = result["currency_code"].ToString();

                    request = webClient.DownloadString(image_uri);
                    result = JsonConvert.DeserializeObject(request) as JObject;
                    image_uri = result["_links"]["ua:images"]["href"].ToString();
                    request = webClient.DownloadString(image_uri);
                    result = JsonConvert.DeserializeObject(request) as JObject;
                    NewCity.ImagePath = result["photos"][0]["image"]["web"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return NewCity;
            }
        }
    }
}
