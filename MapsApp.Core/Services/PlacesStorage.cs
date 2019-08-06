using MapsApp.Core.Models;
using MapsApp.Models;
using MapsApp.Services.Interfaces;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
//using Xamarin.Forms.Maps;

namespace MapsApp.Services
{
    public class PlacesStorage : MvxNotifyPropertyChanged, IPlacesStorage
    {
        private readonly ISettingsService _settingsService;
        private MapSpan _mapSpan;
        private ObservableCollection<Place> _places = new ObservableCollection<Place>();

        public PlacesStorage(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            Initialize = InitializeAsync();
        }

        public ObservableCollection<Place> Places
        {
            get { return _places; }
            private set { SetProperty(ref _places, value); }
        }

        public Task Initialize { get; }

        public MapSpan MapSpan
        {
            get { return _mapSpan; }
            private set { SetProperty(ref _mapSpan, value); }
        }

        public async Task AddPlaceToCollectionAsync(Place place)
        {
            if (Places.All(x => x.Id != place.Id))
            {
                Places.Add(place);
                await _settingsService.SavePlacesAsync(Places);
            }
        }

        public async Task UpdateMapSpanForCurrentPointsAsync()
        {
            //var location = await Geolocation.GetLocationAsync();

            if (Places.Count > 1)
            {
                var tempPlaces = Places.ToList();
                //tempPlaces.Add(new Place(location.Latitude, location.Longitude));

                var westLat = tempPlaces.Min(x => x.Geometry.Location.Latitude);
                var southLng = tempPlaces.Min(x => x.Geometry.Location.Longitude);
                var eastLat = tempPlaces.Max(x => x.Geometry.Location.Latitude);
                var northLng = tempPlaces.Max(x => x.Geometry.Location.Longitude);

                MapSpan = new MapSpan(eastLat, westLat, northLng, southLng);
            }
            //else
                //MapSpan = new MapSpan(location.Latitude, location.Longitude);
        }

        public async Task RemovePlaceFromCollectionAsync(Place place)
        {
            if (Places.Any(x => x.Id == place.Id))
            {
                Places.Remove(place);
                await _settingsService.SavePlacesAsync(Places);
            }
        }

        private async Task InitializeAsync()
        {
            await _settingsService.LoadPlacesAsync().ContinueWith(async (t) =>
            {
                if (t.Result != null)
                {
                    Places = new ObservableCollection<Place>(t.Result);
                    await UpdateMapSpanForCurrentPointsAsync();
                }
            });
        }
    }
}
