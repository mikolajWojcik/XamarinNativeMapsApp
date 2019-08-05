using MapsApp.Models;
using MapsApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapsApp.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly string secureStorageKey = "places-list-location";

        public async Task<IEnumerable<Place>> LoadPlacesAsync()
        {
            var loadedJson = await Xamarin.Essentials.SecureStorage.GetAsync(secureStorageKey);

            if (!string.IsNullOrWhiteSpace(loadedJson))
                return JsonConvert.DeserializeObject<IEnumerable<Place>>(loadedJson);
            else
                return default(IEnumerable<Place>);
        }

        public async Task SavePlacesAsync(IEnumerable<Place> places)
        {
            var jsonToSave = JsonConvert.SerializeObject(places);

            await Xamarin.Essentials.SecureStorage.SetAsync(secureStorageKey, jsonToSave);
        }
    }
}
