using MapsApp.Models;
using MapsApp.Services.Interfaces;
using MvvmCross.Commands;
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

        public MapPageViewModel(IPlacesStorage placesStorage)
        {
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
            ArePermissionsSatisfied = await CheckPermissions();

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

        private async Task<bool> CheckPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        //Device.BeginInvokeOnMainThread(async () =>
                        //{
                        //    await _pageDialog.DisplayAlertAsync("Need location", "Application needs information about mobile location to show it on map", "OK");
                        //});
                    }

                    var permissionsDictionary = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = permissionsDictionary[Permission.Location];
                }

                return status == PermissionStatus.Granted;
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
                return false;
            }
        }

        private bool CanNavigateToAddressesPage()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        public void SearchAddresses()
        {
            //Device.BeginInvokeOnMainThread( async () =>
            //{
            //    var result = await NavigationService.NavigateAsync(nameof(AddressesPage));

            //    if (!result.Success)
            //        Crashes.TrackError(result.Exception);
            //});
        }
    }
}
