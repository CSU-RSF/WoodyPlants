# Colorado Woody Plants App

## Introduction
The Colorado Woody Plants app is based on Xamarin, a Microsoft-owned cross-platform mobile application framework. 
Visual Studio or Xamarin Studio is required to make changes to the iOS and/or Android applications. 
The following are general resources for setup/configuration as well as development with Xamarin:
* [Xamarin Guides](https://developer.xamarin.com/guides/): Excellent resources for Xamarin development
* [Xamarin University](https://university.xamarin.com/): Online courses in foundational principles of Xamarin development (free trial of 30 days)

### Setup
Setup for Xamarin includes either installing Xamarin Studio or Visual Studio with Xamarin. Consult the Xamarin Guides for setup;
keep in mind the following:
* You will need access to a Mac to be able to test your app on a simulator and in order to publish an app to the App Store.
* When using Visual Studio + Xamarin on Windows, it is probably easier to enable and use the [Visual Studio Emulator](https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/debug-on-emulator/visual-studio-android-emulator/) for Android (a built-in emulator) rather than using the standalone Android SDK Emulator. You will need to align the Android SDK accordingly in your VS settings.
* Tasks such as setting up provisioning profiles for iOS development can be tricky, so don't be surprised if you spend a full day or two just getting your development environment setup.

Once you get the app cloned in Visual Studio and your emulators are set up, you should be able to run the application and the app will attempt to download new plant data. From here, you should be off and running with the application.

### Tools & Resources
Key technologies implemented in this app:
* [Xamarin](http://www.xamarin.com) - Cross-platform mobile application framework using C# and .NET
* [Xamarin Forms](https://developer.xamarin.com/guides/xamarin-forms/getting-started/) - Cross-platform UI development toolkit--one UI codebase with translation to native UI components in iOS and Android
* [Portable Class Library (PCL)](https://developer.xamarin.com/guides/cross-platform/application_fundamentals/pcl/) - Allows for usage of shared, platform agnostic, code in a separate project

Several Nuget packages are used to add functionality to the application:
* [SQLite.Net-PCL](https://github.com/oysteinkrog/SQLite.Net-PCL) - PCL version of SQLite-net library to store data in SQLite 3 database; use Async version as well
* [SQLite-Net Extensions](https://bitbucket.org/twincoders/sqlite-net-extensions) - Simple ORM that provides relationship support; use Async version as well
* [Json.NET](http://www.newtonsoft.com/json) - Parses response from MobileAPI server and converts it to objects for storing in the database
* [Connectivity Plugin for Xamarin and Windows](https://github.com/jamesmontemagno/ConnectivityPlugin) - Used to determine current connection status
* [SharpZipLib](https://github.com/ygrenier/SharpZipLib.Portable) - Used for unzipping image files sent over from the MobileAPI server
* [PCLStorage](https://components.xamarin.com/gettingstarted/pclstorage) - Allows for read/write access to platform-specific file structure within the PCL
* [CarouselView control for Xamarin Forms](https://github.com/alexrainman/CarouselView) - This is used to create a Carousel view for plant images, going beyond the Xamarin Forms CarouselPage functionality
* [Geolocator Plugin for Xamarin and Windows](https://github.com/jamesmontemagno/GeolocatorPlugin) - Used to get GPS location of phone for map
* [Xamarin Forms Labs](https://github.com/XLabs/Xamarin-Forms-Labs) - Additional Forms UI components, such as ImageButton used in plants search characteristics
 
Note: Nuget packages, if added to the PCL, must also be added to the platform-specific projects

### Deployment/Publishing
Use the App Store Provisioning Profile (iOS) and Google Play Distribution (Android) supplied by RSF's director. For additional information on publishing an app, consult the following resources:
* [Xamarin Android - Publishing an Application](https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/publishing_an_application/)
* [Xamarin iOS - Deployment and Testing](https://developer.xamarin.com/guides/ios/deployment,_testing,_and_metrics/)

### Additional Documentation
This project has many of the exact same features as the [Colorado Wetlands App](https://github.com/kevindbr/CSUColoradoWetlandsApp), so it should be used as a reference for building this application.

Both the Woody Plants and Wetlands projects are based on the [Portable Ubiquitous Mobile Application](https://github.com/CSU-RSF/PUMA) (PUMA) project, which is designed to "facilitate rapid development of mobile applications with basic UI, database, and mapping components."
It is currently (as of 8/16/2017) substantially behind this application and has less features, but its documentation is worth consulting to give insight into this project as well. 

### TODO
* Work with stakeholders to determine and implement specific info needed on PlantsDetailPage (tabs and text)
* Get images from stakeholders, input and connect to plants in database, store in a zip folder and as individual files on the server (in the MobileApi app under /Resources/Images/Woody)
* Implement download and streaming of plants and images
* Work with stakeholders to get all text/information for "How To Use", "Botanical Help", and "About/Contact"
* Implement search by characteristics feature, which dynamically filters the plant list
* Fix bugs--and double-check readiness with stakeholders--before deployment to production
* Launch on iTunes App Store and Google Play after ensuring that store media (icons, feature graphics, etc.) is as desired by stakeholders