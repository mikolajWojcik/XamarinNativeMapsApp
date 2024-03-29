﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MapsApp.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace MapsApp.Droid.Views
{
    [Activity(Label = "AddressesView")]
    public class AddressesView : MvxActivity<AddressesPageViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddressesPage);
        }
    }
}