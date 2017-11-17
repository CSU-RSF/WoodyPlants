
using PortableApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;

namespace PortableApp
{
    public partial class MainPage : ViewHelpers
    {
        private bool isConnected;
        private bool isConnectedToWiFi;
        private Grid innerContainer;
        private Switch downloadImagesSwitch;
        private WoodySetting downloadImagesSetting;
        private int numberOfPlants;
        private bool updatePlants = false;
        DownloadWoodyPlantsPage downloadPage;
        private bool finishedDownload = false;
        private bool canceledDownload = false;
        private WoodySetting datePlantDataUpdatedLocally;
        private WoodySetting datePlantDataUpdatedOnServer;
        private List<WoodySetting> imageFilesToDownload = new List<WoodySetting>();
        private IEnumerable<WoodySetting> imageFileSettingsOnServer;

        protected override async void OnAppearing()
        {
            // Initialize variables
            isConnected = Connectivity.checkConnection();
            isConnectedToWiFi = Connectivity.checkWiFiConnection();
            numberOfPlants = new List<WoodyPlant>(App.WoodyPlantRepo.GetAllWoodyPlants()).Count;
            downloadImagesSetting = await App.WoodySettingsRepo.GetSettingAsync("Download Images");
            downloadImages = (bool)downloadImagesSetting.valuebool;
            //downloadImagesSwitch.IsToggled = downloadImages;


            if (numberOfPlants>0)
            {
                downloadImagesSwitch.IsToggled = true;
            }
            else
            {
                downloadImagesSwitch.IsToggled = false;
            }


            // in order to go to the DownloadPage, must be connected to the internet (or cell data), did not just come from the download page
            if (isConnected && !canceledDownload && !finishedDownload)
            {
                datePlantDataUpdatedLocally = App.WoodySettingsRepo.GetSetting("Date Plants Downloaded");
                try
                {
                    datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                    imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                    ImageFilesToDownload();
                }
                catch { }

                // If valid date comparison and date on server is more recent than local date, show download button
                if (datePlantDataUpdatedLocally != null && datePlantDataUpdatedOnServer != null)
                {
                    if (datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp || numberOfPlants == 0)
                    {
                        updatePlants = true;
                        ToDownloadPage();
                    }
                }
                
            }
            base.OnAppearing();
        }

        public MainPage()
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container            
            Grid navigationBar = ConstructNavigationBarMain("CO Woody Plants");
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;


            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


            // Add navigation buttons
            Button plantsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "PLANTS"
            };
            plantsButton.Clicked += ToPlants;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(plantsButton, 0, 2);

            Button helpButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "BOTANICAL HELP"
            };
            helpButton.Clicked += ToHelp;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(helpButton, 0, 3);

            Button howToUseButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "HOW TO USE"
            };

            howToUseButton.Clicked += ToHowToUse;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(howToUseButton, 0, 4);

            Button aboutButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "ABOUT/CONTACT"
            };

            aboutButton.Clicked += ToAbout;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(aboutButton, 0, 5);

            // Switch for downloading images
            StackLayout downloadImagesLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Margin = new Thickness(20, 0, 20, 0), HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            downloadImagesSwitch = new Switch { BackgroundColor = Color.FromHex("66000000") };
            downloadImagesSwitch.Toggled += ToggleDownloadImagesSwitch;
            Label downloadImagesLabel = new Label { Text = "Download Plants", TextColor = Color.White };
            downloadImagesLayout.Children.Add(downloadImagesLabel);
            downloadImagesLayout.Children.Add(downloadImagesSwitch);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(downloadImagesLayout, 0, 6);

            // Add empty space
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });


        }

        private async void HandleFinishedDownload(object sender, EventArgs e)
        {
            finishedDownload = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void HandleCancelDownload(object sender, EventArgs e)
        {
            canceledDownload = true;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void ToDownloadPage()
        {
            downloadPage = new DownloadWoodyPlantsPage(updatePlants, datePlantDataUpdatedLocally, datePlantDataUpdatedOnServer);
            updatePlants = true;
            downloadPage.InitCancelDownload += HandleCancelDownload;
            downloadPage.InitFinishedDownload += HandleFinishedDownload;
            await Navigation.PushModalAsync(downloadPage);
           
        }

        public void ImageFilesToDownload()
        {
            foreach (WoodySetting imageFile in imageFileSettingsOnServer)
            {
                WoodySetting imageFileLocalSetting = App.WoodySettingsRepo.GetImageZipFileSetting(imageFile.valuetext);
                if (imageFileLocalSetting == null)
                    imageFilesToDownload.Add(imageFile);
            }
        }

        private async void ToggleDownloadImagesSwitch(object sender, ToggledEventArgs e)
        {
            if (downloadImagesSwitch.IsToggled == true)
            {
                //downloadImagesSetting.valuebool = true;
                //ToDownloadPage();
                if (imageFilesToDownload.Count > 0|| numberOfPlants ==0)
                    ToDownloadPage();
            }
            else
                downloadImagesSetting.valuebool = false;

            await App.WoodySettingsRepo.AddOrUpdateSettingAsync(downloadImagesSetting);
        }
    }
}
