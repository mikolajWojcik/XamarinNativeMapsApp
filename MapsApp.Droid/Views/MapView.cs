using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MapsApp.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace MapsApp.Droid.Views
{
    [Activity(Label = "@string/activity_label_mapwithmarkers", MainLauncher = true)]
    public class MapView : MvxActivity<MapPageViewModel>, IOnMapReadyCallback
    {
        private GoogleMap _googleMap;

        public async void OnMapReady(GoogleMap map)
        {
            _googleMap = map;

            _googleMap.UiSettings.ZoomControlsEnabled = true;
            _googleMap.UiSettings.CompassEnabled = true;
            _googleMap.UiSettings.MyLocationButtonEnabled = true;

            AddMarkersToMap();
            await UpdateMapCamera();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MapsPage);

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation }, 1);
            }
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessCoarseLocation }, 2);
            }
        }

        void AddMarkersToMap()
        {
            foreach(var position in ViewModel.Places)
            {
                var latLng = new LatLng(position.Geometry.Location.Latitude, position.Geometry.Location.Longitude);

                var marker = new MarkerOptions();
                marker.SetPosition(latLng)
                    .SetTitle(position.Name)
                    .SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));

                _googleMap.AddMarker(marker);
            }
        }

        private async Task UpdateMapCamera()
        {
            await ViewModel.PlacesStorage.UpdateMapSpanForCurrentPointsAsync().ContinueWith((t) =>
            {
                if (ViewModel.PlacesStorage.MapSpan != null)
                {
                    var southWest = new LatLng(ViewModel.PlacesStorage.MapSpan.WestLat, ViewModel.PlacesStorage.MapSpan.SouthLng);
                    var northEast = new LatLng(ViewModel.PlacesStorage.MapSpan.EastLat, ViewModel.PlacesStorage.MapSpan.NorthLng);

                    var latLangBounds = new LatLngBounds(southWest, northEast);

                    var cameraUpdate = CameraUpdateFactory.NewLatLngBounds(latLangBounds, 100);

                    this.RunOnUiThread(() => _googleMap.MoveCamera(cameraUpdate));
                }
            });           
        }
    }
}