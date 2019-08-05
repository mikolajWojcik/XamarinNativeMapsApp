using MapsApp.DTO;
using MapsApp.Models;
using MapsApp.Models.Enums;
using MapsApp.Services.Interfaces;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MapsApp.Services
{
    public class GooglePlacesService : MvxNotifyPropertyChanged, IGooglePlacesService
    {
        private readonly string _googlePlacesAPI = "https://maps.googleapis.com/maps/api/place/textsearch/json";
        private readonly string _apiKey = "[API_KEY]";
        private readonly HttpClient _httpClient;
        private bool _isNextPageAviable;
        private string _nextPageToken;
        private GoogleApiStatusCode _currentRequestStatus;

        public GooglePlacesService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{_googlePlacesAPI}?key={_apiKey}")
            };
        }

        public bool IsNextPageAvailable
        {
            get => _isNextPageAviable; 
            private set => SetProperty(ref _isNextPageAviable, value); 
        }

        public GoogleApiStatusCode CurrentRequestStatus
        {
            get => _currentRequestStatus; 
            private set => SetProperty(ref _currentRequestStatus, value); 
        }

        public async Task<IEnumerable<Place>> GetPlacesAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException();

            return await GetPlacesForQueryArgument("query", name);
        }

        private async Task<IEnumerable<Place>> GetPlacesForQueryArgument(string argumentName, string argumentValue)
        {
            string requestUrl = FormatRequestUrl(argumentName, argumentValue);
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();

            return HandleResponseBody(responseBody);
        }

        public async Task<IEnumerable<Place>> LoadMorePlacesAsync()
        {
            if (!IsNextPageAvailable)
                throw new InvalidOperationException("Next page is not aviable");

            return await GetPlacesForQueryArgument("pagetoken", _nextPageToken);
        }

        private IEnumerable<Place> HandleResponseBody(string responseBody)
        {
            var responseObject = JsonConvert.DeserializeObject<PlacesResponse>(responseBody);

            if(string.IsNullOrEmpty(responseObject.NextPageToken))
            {
                IsNextPageAvailable = false;
                _nextPageToken = string.Empty;                
            }
            else
            {
                IsNextPageAvailable = true;
                _nextPageToken = responseObject.NextPageToken;
            }

            CurrentRequestStatus = responseObject.Status;

            return responseObject.Places;
        }

        private string FormatRequestUrl(string argumentName, string value)
        {
            var encodedName = HttpUtility.UrlEncode(value);
            var requestUrl = $"{_httpClient.BaseAddress}&{argumentName}={(encodedName)}";
            return requestUrl;
        }
    }
}
