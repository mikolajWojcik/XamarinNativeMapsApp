using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
//using Xamarin.Forms.Maps;

namespace MapsApp.Models
{
    public class Place
    {
        public Place()
        {

        }

        public Place(double lat, double lng)
        {
            Geometry = new Geometry { Location = new Location { Latitude = lat, Longitude = lng } };
        }

        [JsonProperty("formatted_address")]
        public string FormattedAddres { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonIgnore]
        //public Position Position
        //{
        //    get => new Position(Geometry.Location.Latitude, Geometry.Location.Longitude);
        //}
    }
}
