﻿using System;
using System.Collections.Generic;
using System.Linq;
//using SQLite.Platform.XamarinIOS;
using Foundation;
using UIKit;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;

namespace PortableApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CarouselViewRenderer.Init();
            CachedImageRenderer.Init();
            UIApplication.SharedApplication.IdleTimerDisabled = true;


            string dbPath = FileAccessHelper.GetLocalFilePath("db.db3");
            // var platform = new SQLitePlatformIOS();
            LoadApplication(new App(dbPath));

            return base.FinishedLaunching(app, options);
        }

        /* public override UIWindow Window
         {
             get;
             set;
         }*/
    }
}