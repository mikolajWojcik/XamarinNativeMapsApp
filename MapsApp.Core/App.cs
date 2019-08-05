using MapsApp.Services;
using MapsApp.Services.Interfaces;
using MapsApp.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapsApp.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IGooglePlacesService, GooglePlacesService>();
            Mvx.IoCProvider.RegisterType<ISettingsService, SettingsService>();
            Mvx.IoCProvider.RegisterType<IPlacesStorage, PlacesStorage>();

            RegisterAppStart<MapPageViewModel>();
        }
    }
}
