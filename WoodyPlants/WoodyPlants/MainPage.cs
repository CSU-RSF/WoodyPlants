
using PortableApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;
using System.ComponentModel;
using XLabs.Forms.Controls;
using System.Diagnostics;

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
        private Button downloadImagesButton = new Button { Style = Application.Current.Resources["semiTransparentButton"] as Style, Text = "Trying To Connect To Server..." };
        private Label downloadImagesLabel = new Label { TextColor = Color.White, BackgroundColor = Color.Transparent };


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private string downloadButtonText = "Download Plant DB";
        public string DownloadButtonText
        {
            get
            {
                return this.downloadButtonText;
            }

            set
            {
                this.downloadButtonText = value;
                downloadImagesLabel.Text = this.downloadButtonText;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadButtonText"));
            }
        }


        //Start here
        protected override async void OnAppearing()
        {
            if (!canceledDownload)
            {
                // Initialize variables
            isConnected = Connectivity.checkConnection();
            isConnectedToWiFi = Connectivity.checkWiFiConnection();
            numberOfPlants = new List<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants()).Count;
            //downloadImagesSetting = await App.WoodySettingsRepo.GetSettingAsync("Download Images");
           //downloadImages = (bool)downloadImagesSetting.valuebool;

                // if connected to WiFi and updates are needed
                if (isConnected)
                {
                    datePlantDataUpdatedLocally = App.WoodySettingsRepo.GetSetting("Date Plants Downloaded");
                    try
                    {
                        datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                        //imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                       // ImageFilesToDownload();

                        if (datePlantDataUpdatedLocally.valuetimestamp == datePlantDataUpdatedOnServer.valuetimestamp)
                        {
                            DownloadButtonText = "Plant DB Up To Date";
                            downloadImagesButton.Text = "(Local Database Up To Date)";
                            downloadImagesLabel.TextColor = Color.Green;
                            updatePlants = false;
                        }
                        else
                        {
                            if (numberOfPlants == 0)
                            {
                                DownloadButtonText = "Download Plant DB";
                                downloadImagesButton.Text = "Download Plants (No Local Database)";
                                downloadImagesLabel.TextColor = Color.Red;
                                updatePlants = true;
                            }
                            else
                            {
                                DownloadButtonText = "New Plant DB Available";
                                downloadImagesButton.Text = "Re-Sync Plants (New Database Available)";
                                downloadImagesLabel.TextColor = Color.Yellow;
                                updatePlants = true;
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
                    }
                }
                else
                {
                    if (numberOfPlants == 0)
                    {
                        await DisplayAlert("No Local Database Detected", "Please connect to WiFi or cell network to download or use CO Wetlands App", "OK");
                        updatePlants = false;
                    }
                    else
                    {
                        downloadImagesButton.Text = "No Internet Connection";
                    }
                }
            }
            else
            {
                canceledDownload = false;
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
     

            Button searchButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Id By Plant Characteristic"
            };
            searchButton.Clicked += ToSearch;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55) });
            innerContainer.Children.Add(searchButton, 0, 2);

            // Add navigation buttons
            Button plantsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Id by Family/Species"
            };
            plantsButton.Clicked += ToPlants;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55) });
            innerContainer.Children.Add(plantsButton, 0, 3);


            Button howToUseButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Favorites/Browse"
            };

            howToUseButton.Clicked += ToFavorites;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55) });
            innerContainer.Children.Add(howToUseButton, 0, 4);

            Button aboutButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "About"
            };

            aboutButton.Clicked += ToAbout;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55) });
            innerContainer.Children.Add(aboutButton, 0, 5);

            Button linksButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Links and Careers in Natural Resources"
            };

            linksButton.Clicked += ToLink;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(55) });
            innerContainer.Children.Add(linksButton, 0, 6);
 

            StackLayout downloadImagesLayout = new StackLayout { BackgroundColor = Color.Transparent, Orientation = StackOrientation.Vertical, Padding = new Thickness(5, 5, 5, 5), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };
            downloadImagesButton.Clicked += ClickDownloadImages;
            downloadImagesLayout.Children.Add(downloadImagesButton);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(85) });
            innerContainer.Children.Add(downloadImagesLayout, 0, 7);


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
            downloadPage.InitCancelDownload += HandleCancelDownload;
            downloadPage.InitFinishedDownload += HandleFinishedDownload;
            await Navigation.PushModalAsync(downloadPage);

        }

        /*public void ImageFilesToDownload()
        {
            foreach (WoodySetting imageFile in imageFileSettingsOnServer)
            {
                WoodySetting imageFileLocalSetting = App.WoodySettingsRepo.GetImageZipFileSetting(imageFile.valuetext);
                if (imageFileLocalSetting == null)
                    imageFilesToDownload.Add(imageFile);
            }
        }*/

        private async void ClickDownloadImages(object sender, EventArgs e)
        {

            // If valid date comparison and date on server is more recent than local date, show download button
            if (datePlantDataUpdatedLocally != null && datePlantDataUpdatedOnServer != null)
            {
                if (datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp || numberOfPlants == 0)
                {
                    updatePlants = true;
                    ToDownloadPage();
                }
            }



            await App.WoodySettingsRepo.AddOrUpdateSettingAsync(downloadImagesSetting);
        }
    
    }
}
