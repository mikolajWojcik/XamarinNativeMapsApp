using MapsApp.Models;
using MapsApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MapsApp.Services.Interfaces
{
    public interface IGooglePlacesService : INotifyPropertyChanged
    {
        Task<IEnumerable<Place>> GetPlacesAsync(string name);

        Task<IEnumerable<Place>> LoadMorePlacesAsync();

        bool IsNextPageAvailable { get; }

        GoogleApiStatusCode CurrentRequestStatus { get; }
    }
}
