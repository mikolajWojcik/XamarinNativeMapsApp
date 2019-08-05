using MapsApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MapsApp.Services.Interfaces
{
    public interface ISettingsService 
    {
        Task<IEnumerable<Place>> LoadPlacesAsync();

        Task SavePlacesAsync(IEnumerable<Place> places);
    }
}
