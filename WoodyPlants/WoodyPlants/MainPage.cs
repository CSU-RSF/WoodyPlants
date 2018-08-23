
using PortableApp.Data;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using PortableApp.Models;
using System.ComponentModel;
using System.Diagnostics;
using PCLStorage;
using System.Collections.ObjectModel;

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
        private bool resyncPlants = false;
        private bool clearDatabase = false;
        private WoodySetting datePlantDataUpdatedLocally;
        private WoodySetting datePlantDataUpdatedOnServer;
        private List<WoodySetting> imageFilesToDownload = new List<WoodySetting>();
        private IEnumerable<WoodySetting> imageFileSettingsOnServer;
        private Button downloadImagesButton = new Button { Style = Application.Current.Resources["semiTransparentButton"] as Style, Text = "Trying To Connect To Server..." };
        private Label downloadImagesLabel = new Label { TextColor = Color.White, BackgroundColor = Color.Transparent };
        private Label streamingLabel = new Label { Text = "You Are Streaming Plants", Style = Application.Current.Resources["sectionHeader"] as Style, HorizontalOptions = LayoutOptions.CenterAndExpand, Margin = new Thickness(0, 0, 0, 0) };


        IFolder rootFolder = FileSystem.Current.LocalStorage;

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
                // Initiate variables
                isConnected = Connectivity.checkConnection();
                isConnectedToWiFi = Connectivity.checkWiFiConnection();
                downloadImagesSetting = await App.WoodySettingsRepo.GetSettingAsync("Download Images");
                //downloadImages = (bool)downloadImagesSetting.valuebool;

                //numberOfPlants = new List<WoodyPlant>(App.WoodyPlantRepo.GetAllWoodyPlants()).Count;

                // if connected to WiFi and updates are needed
                if (isConnected)
                {
                    datePlantDataUpdatedLocally = App.WoodySettingsRepo.GetSetting("Date Plants Downloaded");
                    try
                    {
                        datePlantDataUpdatedOnServer = await externalConnection.GetDateUpdatedDataOnServer();
                        imageFileSettingsOnServer = await externalConnection.GetImageZipFileSettings();
                        ImageFilesToDownload();

                        if (datePlantDataUpdatedLocally.valuetimestamp == datePlantDataUpdatedOnServer.valuetimestamp)
                        {
                            DownloadButtonText = "Plant DB Up To Date";
                            downloadImagesButton.Text = "Clear Database And Stream Plants";
                            streamingLabel.Text = "You Are Using Your Local Plant Database";
                            downloadImagesLabel.TextColor = Color.Green;
                            updatePlants = false;
                            resyncPlants = false;
                            clearDatabase = true;
                            downloadImages = false;
                        }
                        else
                        {
                            if (datePlantDataUpdatedLocally.valuetimestamp == null)
                            {
                                DownloadButtonText = "Download Plant DB";
                                downloadImagesButton.Text = "Download Plant Database";
                                streamingLabel.Text = "You Are Streaming Plants";
                                downloadImagesLabel.TextColor = Color.Red;
                                updatePlants = true;
                                resyncPlants = false;
                                clearDatabase = false;
                                downloadImages = true;
                            }
                            else if ((datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp) && datePlantDataUpdatedLocally.valuetimestamp != null)
                            {
                                DownloadButtonText = "New Plant DB Available";
                                downloadImagesButton.Text = "Download Database Update";
                                streamingLabel.Text = "You Are Using Your Local Plant Database";
                                downloadImagesLabel.TextColor = Color.Yellow;
                                updatePlants = true;
                                resyncPlants = true;
                                clearDatabase = false;
                                downloadImages = false;
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
                        await DisplayAlert("No Local Database Detected", "Please connect to WiFi or cell network to download or use CO Woodys App", "OK");
                        updatePlants = false;
                        resyncPlants = false;
                        clearDatabase = false;
                        downloadImages = true;
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

            //ObservableCollection<WoodyPlant> plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());
           
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
                Text = "Identify By Plant Characteristic"
            };
            searchButton.Clicked += ToSearch;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
            innerContainer.Children.Add(searchButton, 0, 2);

            // Add navigation buttons
            Button plantsButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Identify By Common/Scientific Name"
            };
            plantsButton.Clicked += ToPlants;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
            innerContainer.Children.Add(plantsButton, 0, 3);


            Button howToUseButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Favorites"
            };

            howToUseButton.Clicked += ToFavorites;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
            innerContainer.Children.Add(howToUseButton, 0, 4);

            Button aboutButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "About"
            };

            aboutButton.Clicked += ToAbout;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
            innerContainer.Children.Add(aboutButton, 0, 5);

            Button linksButton = new Button
            {
                Style = Application.Current.Resources["semiTransparentButton"] as Style,
                Text = "Links and Careers in Natural Resources"
            };

            linksButton.Clicked += ToLink;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(65) });
            innerContainer.Children.Add(linksButton, 0, 6);


            StackLayout downloadImagesLayout = new StackLayout { BackgroundColor = Color.Transparent, Orientation = StackOrientation.Vertical, Padding = new Thickness(5, 5, 5, 0), HorizontalOptions = LayoutOptions.CenterAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand };

            //Button to download images

            downloadImagesButton.Clicked += DownloadImagesPressed;
            downloadImagesLayout.Children.Add(downloadImagesButton);

            //  downloadImagesLayout.Children.Add(downloadImagesLabel);

            downloadImagesLayout.Children.Add(streamingLabel);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            innerContainer.Children.Add(downloadImagesLayout, 0, 7);


            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

           


        }

        private async void ToDownloadPage()
        {
            downloadPage = new DownloadWoodyPlantsPage(updatePlants, datePlantDataUpdatedLocally, datePlantDataUpdatedOnServer, imageFilesToDownload, downloadImages, resyncPlants, clearDatabase);
            downloadPage.InitCancelDownload += HandleCancelDownload;
            downloadPage.InitFinishedDownload += HandleFinishedDownload;
            await Navigation.PushModalAsync(downloadPage);
        }

        private async void HandleFinishedDownload(object sender, EventArgs e)
        {
            finishedDownload = true;
            datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
            await App.WoodySettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void HandleCancelDownload(object sender, EventArgs e)
        {
            canceledDownload = true;
            //ClearRepositories();
            //ClearLocalRepositories();
            try
            {
                IFolder folder = await rootFolder.GetFolderAsync("Images");
                await folder.DeleteAsync();
            }
            catch (Exception exception) { }

            await App.Current.MainPage.Navigation.PopModalAsync();
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

        private async void DownloadImagesPressed(object sender, EventArgs e)
        {
            if (clearDatabase)
            {
                var answer = await DisplayAlert("Warning", "Are you sure you want to clear your database and stream plants?", "Yes", "No");
                if (answer)
                {
                    DownloadButtonText = "Plant DB Up To Date";
                    downloadImagesButton.Text = "Clearing Local Database...";
                    try
                    {
                        IFolder folder = await rootFolder.GetFolderAsync("Images");
                        await folder.DeleteAsync();
                    }
                    catch (Exception exception) { }

                    ClearRepositories();
                    ClearLocalRepositories();
                    updatePlants = false;
                    resyncPlants = false;
                    clearDatabase = false;

                    datePlantDataUpdatedLocally.valuetimestamp = null;
                    await App.WoodySettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);

                    DownloadButtonText = "Download Plant DB";
                    downloadImagesButton.Text = "Download Plants";
                    streamingLabel.Text = "You Are Streaming Plants";
                    downloadImagesLabel.TextColor = Color.Red;

                    downloadImagesSetting.valuebool = true;
                    downloadImages = true;

                }
            }
            // If valid date comparison and date on server is more recent than local date, show download button
            else if (datePlantDataUpdatedOnServer.valuetimestamp != null)
            {
                if ((datePlantDataUpdatedLocally.valuetimestamp < datePlantDataUpdatedOnServer.valuetimestamp) || numberOfPlants == 0 || datePlantDataUpdatedLocally.valuetimestamp == null)
                {                 
                    updatePlants = true;
                    ToDownloadPage();
                    downloadImagesSetting.valuebool = false;
                    downloadImages = false;
                }
            }
            await App.WoodySettingsRepo.AddOrUpdateSettingAsync(downloadImagesSetting);


        }

        private void ClearRepositories()
        {
            //Clear Repositories
            App.WoodyPlantRepo.ClearWoodyPlants();
           // App.WoodyPlantImageRepo.ClearWoodyImages();
            App.WoodySettingsRepo.ClearWoodySettings();


        }

        private void ClearLocalRepositories()
        {
            try { App.WoodyPlantRepoLocal.ClearWoodyPlantsLocal(); } catch (Exception e) { }
           // try { App.WoodyPlantImageRepoLocal.ClearWoodyImagesLocal(); } catch (Exception e) { }
        }

    }
}
