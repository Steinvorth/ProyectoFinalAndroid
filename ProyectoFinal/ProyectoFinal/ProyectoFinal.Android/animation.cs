﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoFinal.Droid
{
    [Activity(Label = "TuProyecto", Icon = "@mipmap/icon", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class animation : Activity
    {
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

           
            OverridePendingTransition(0, 0);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            base.OnPostCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context,typeof(MainActivity)));
        }

       

    }
}