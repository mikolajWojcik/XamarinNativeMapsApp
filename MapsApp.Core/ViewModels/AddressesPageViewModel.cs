using MapsApp.Models;
using MapsApp.Services.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapsApp.ViewModels
{
	public class AddressesPageViewModel : MvxViewModel
	{
        private readonly IPlacesStorage _storage;
        private MvxObservableCollection<Place> _places;
        private bool _isLableVisible;
        private string _searchText;
        private ICommand _placeSelectedCommand;

        public AddressesPageViewModel(IGooglePlacesService placesService, IPlacesStorage storage) 
        {
            PlacesService = placesService;
            _storage = storage;
        }

        public bool IsLableVisible
        {
            get =>_isLableVisible; 
            set => SetProperty(ref _isLableVisible, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value, OnSearchTextChanged);
        }

        private async void OnSearchTextChanged()
        {
            if (!string.IsNullOrEmpty(SearchText))
                await SearchTextAsync(SearchText);
        }

        public MvxObservableCollection<Place> Places
        {
            get => _places; 
            set => SetProperty(ref _places, value); 
        }

        public IGooglePlacesService PlacesService { get; }

        public ICommand PlaceSelectedCommand => _placeSelectedCommand ?? (_placeSelectedCommand = new MvxCommand<Place>(SelectedPlace, (t) => t != null));

        public override async Task Initialize()
        {
            await base.Initialize();

            Places = new MvxObservableCollection<Place>();
        }

        private async Task SearchTextAsync(string text)
        {
            Places = new MvxObservableCollection<Place>(await PlacesService.GetPlacesAsync(text));
            IsLableVisible = Places.Count == 0;
        }

        private async void SelectedPlace(Place place)
        {
            await _storage.AddPlaceToCollectionAsync(place);

            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    var result = await NavigationService.GoBackAsync();
            //});           
        }
    }
}
