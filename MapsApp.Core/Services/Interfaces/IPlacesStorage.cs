using MapsApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
//using Xamarin.Forms.Maps;

namespace MapsApp.Services.Interfaces
{
    public interface IPlacesStorage : IAsyncInitialization
    {
        ObservableCollection<Place> Places { get; }

        //MapSpan MapSpan { get; }

        Task AddPlaceToCollectionAsync(Place place);

        Task RemovePlaceFromCollectionAsync(Place place);

        Task UpdateMapSpanForCurrentPointsAsync();
    }
}
