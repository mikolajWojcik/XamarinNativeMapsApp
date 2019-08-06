using MapsApp.Models;
using MapsApp.Services.Interfaces;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MapsApp.ViewModels
{
    public class MapPageViewModel : MvxViewModel
    {
        private bool _isInternetAccessible;
        private bool _arePermissionsSatisfied;
        private MvxObservableCollection<Place> _places;
        private readonly IMvxNavigationService _navigationService;

        public MapPageViewModel(IMvxNavigationService navigationService, IPlacesStorage placesStorage)
        {
            _navigationService = navigationService;
            PlacesStorage = placesStorage;
            SearchAddressesCommand = new MvxCommand(SearchAddresses, CanNavigateToAddressesPage);
            IsInternetAccessible = Connectivity.NetworkAccess == NetworkAccess.Internet;

            Connectivity.ConnectivityChanged += (sender, e) =>
            {
                IsInternetAccessible = e.NetworkAccess == NetworkAccess.Internet;
                SearchAddressesCommand.RaiseCanExecuteChanged();
            };
        }

        public bool ArePermissionsSatisfied
        {
            get => _arePermissionsSatisfied; 
            set => SetProperty(ref _arePermissionsSatisfied, value); 
        }

        public bool IsInternetAccessible
        {
            get => _isInternetAccessible; 
            set => SetProperty(ref _isInternetAccessible, value); 
        }

        public MvxObservableCollection<Place> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }

        public IMvxCommand SearchAddressesCommand { get; private set; }
        public IPlacesStorage PlacesStorage { get; }

        public async override Task Initialize()
        {
            await base.Initialize();
            await PlacesStorage.Initialize;

            Places = new MvxObservableCollection<Place>(PlacesStorage.Places);
            PlacesStorage.Places.CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                    foreach (Place item in e.NewItems)
                        Places.Add(item);

                if (e.OldItems != null)
                    foreach (Place item in e.OldItems)
                        if (item != null && Places.Contains(item))
                            Places.Remove(item);

            };
        }

        private bool CanNavigateToAddressesPage()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public async void SearchAddresses()
        {
            await _navigationService.Navigate(typeof(AddressesPageViewModel));    
        }
    }
}
