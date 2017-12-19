using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using PCLStorage;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Core;
using System.Threading;

namespace PortableApp
{
    public partial class DownloadWoodyPlantsPage : ViewHelpers
    {
        public EventHandler InitFinishedDownload;
        public EventHandler InitCancelDownload;
        bool updatePlants;
        WoodySetting datePlantDataUpdatedLocally;
        WoodySetting datePlantDataUpdatedOnServer;
        List<WoodySetting> imageFilesToDownload;
        ObservableCollection<WoodyPlant> plants;
        //ObservableCollection<WoodyGlossary> terms;
        ProgressBar progressBar = new ProgressBar();
        Label downloadLabel = new Label { Text = "", TextColor = Color.White, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center };
        Button cancelButton;
        CancellationTokenSource tokenSource;
        CancellationToken token;
        HttpClient client = new HttpClient();

        protected async override void OnAppearing()
        {
            // Initialize CancellationToken
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            // Initialize progressbar and page
            progressBar.Progress = 0;
            base.OnAppearing();

            // Get all plants from external API call, store them in a collection
            plants = new ObservableCollection<WoodyPlant>(await externalConnection.GetAllPlants());
            //terms = new ObservableCollection<WoodyGlossary>(await externalConnection.GetAllTerms());

            // Save plants to the database
            if (updatePlants && !token.IsCancellationRequested)
                await UpdatePlants(token);

            // Save images to the database
            //if (imageFilesToDownload.Count > 0 && downloadImages && !token.IsCancellationRequested)
            //    await UpdatePlantImages(token);

            FinishDownload();
        }

        public DownloadWoodyPlantsPage(bool updatePlantsNow, WoodySetting dateLocalPlantDataUpdated, WoodySetting datePlantDataUpdated)
        {
            updatePlants = updatePlantsNow;
            datePlantDataUpdatedLocally = dateLocalPlantDataUpdated;
            datePlantDataUpdatedOnServer = datePlantDataUpdated;
            //imageFilesToDownload = (imageFilesNeedingDownloaded == null) ? new List<WoodySetting>() : imageFilesNeedingDownloaded;
            //downloadImages = downloadImagesFromServer;

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid
            {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000")
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add label
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
            innerContainer.Children.Add(downloadLabel, 0, 1);

            // Add progressbar
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(progressBar, 0, 2);

            // Add dismiss button
            cancelButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CANCEL",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            cancelButton.Clicked += CancelDownload;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(cancelButton, 0, 3);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        private void FinishDownload()
        {
            InitFinishedDownload?.Invoke(this, EventArgs.Empty);
        }

        private void CancelDownload(object sender, EventArgs e)
        {
            tokenSource.Cancel();
            InitCancelDownload?.Invoke(this, EventArgs.Empty);
        }

        // Get plants from MobileApi server and save locally
        public async Task UpdatePlants(CancellationToken token)
        {
            try
            {
                downloadLabel.Text = "Downloading Plants...";
                int plantsSaved = 0;
                foreach (var plant in plants)
                {
                    if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                    await App.WoodyPlantRepo.AddOrUpdatePlantAsync(plant);
                    plantsSaved += 1;
                    await progressBar.ProgressTo((double)plantsSaved / (plants.Count), 1, Easing.Linear);
                    Double percent = (double)plantsSaved / (plants.Count);
                    downloadLabel.Text = "Downloading Plant Database..." + Math.Round(percent * 100) + "%";
                }
                //foreach (var term in terms)
                //{
                //    if (token.IsCancellationRequested) { token.ThrowIfCancellationRequested(); };
                //    //await App.WoodyGlossaryRepo.AddOrUpdateTermAsync(term);
                //    plantsSaved += 1;
                //    await progressBar.ProgressTo((double)plantsSaved / (plants.Count + terms.Count), 1, Easing.Linear);
                //}

                downloadLabel.Text = "Download Finished!";
                datePlantDataUpdatedLocally.valuetimestamp = datePlantDataUpdatedOnServer.valuetimestamp;
                await App.WoodySettingsRepo.AddOrUpdateSettingAsync(datePlantDataUpdatedLocally);

                App.WoodyPlantRepoLocal = new WoodyPlantRepositoryLocal(App.WoodyPlantRepo.GetAllWoodyPlants());
            }
            catch (OperationCanceledException e)
            {
                downloadLabel.Text = "Download Canceled!";
                Debug.WriteLine("Canceled UpdatePlants {0}", e.Message);
            }
            catch (Exception e)
            {
                downloadLabel.Text = "Error While Downloading Database!";
                Debug.WriteLine("ex {0}", e.Message);
            }
        }

    }
}
