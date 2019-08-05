using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapsApp.Models
{
    public class Location
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}
