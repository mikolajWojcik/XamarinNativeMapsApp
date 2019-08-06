using System;
using System.Collections.Generic;
using System.Text;

namespace MapsApp.Core.Models
{
    public class MapSpan
    {
        public double EastLat { get; set; }
        public double WestLat { get; set; }
        public double NorthLng { get; set; }
        public double SouthLng { get; set; }

        public MapSpan(double eastLat, double westLat, double northLng, double southLng)
        {
            EastLat = eastLat;
            WestLat = westLat;
            NorthLng = northLng;
            SouthLng = southLng;
        }

        public MapSpan(double lng, double lat)
        {
            EastLat = lat;
            WestLat = lat;
            NorthLng = lng;
            SouthLng = lng;
        }
    }
}
