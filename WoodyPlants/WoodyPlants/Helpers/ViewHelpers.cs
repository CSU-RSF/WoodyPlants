using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;
using PortableApp.Views;
using System.ComponentModel;
using System.Diagnostics;

namespace PortableApp
{
    public class TransparentWebView : WebView
    {
    }

    public class ViewHelpers : ContentPage, INotifyPropertyChanged
    {
       

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private String isLoadingMessage = "Loading";
        public String IsLoadingMessage
        {
            get
            {
                return this.isLoadingMessage;
            }

            set
            {
                this.isLoadingMessage = value;
                RaisePropertyChanged("IsLoadingMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string pName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(pName));
        }


        //
        // VIEWS
        //

        public ExternalDBConnection externalConnection = new ExternalDBConnection();
        //public bool downloadImages = (bool)App.WoodySettingsRepo.GetSetting("Download Images").valuebool;

        public bool downloadImages =true;

        // Construct Page Container as an AbsoluteLayout with a background image
        public AbsoluteLayout ConstructPageContainer()
        {
            AbsoluteLayout pageContainer = new AbsoluteLayout { BackgroundColor = Color.Black };
            Image backgroundImage = new Image {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Images.background.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.7
            };

            var loadingLabel = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.White
            };

            loadingLabel.BindingContext = this;
            loadingLabel.SetBinding(Label.TextProperty, "IsLoadingMessage", BindingMode.TwoWay);
            loadingLabel.SetBinding(Label.IsVisibleProperty, "IsLoading", BindingMode.OneWay);
            loadingLabel.TextColor = Color.White;


            var indicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            indicator.Color = Color.Blue;
            indicator.BindingContext = this;
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading", BindingMode.TwoWay);

            var grid = new Grid();

            //  grid.BackgroundColor = IsLoading ? Color.Black: Color.Transparent;
            //  grid.Opacity = .2;

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(indicator, 0, 1);
            grid.Children.Add(loadingLabel, 0, 0);
            Grid.SetColumnSpan(loadingLabel, 2);
            Grid.SetColumnSpan(indicator, 2);

            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            pageContainer.Children.Add(grid, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            return pageContainer;
        }
        


        public Grid ConstructNavigationBar(string titleText)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }

        public Grid ConstructNavigationBarMain(string titleText)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            //gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }

        public Grid ConstructPlantNavigationBar(string titleText, WoodyPlant plant, ObservableCollection<WoodyPlant> plants)
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            //Favorite icon
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(FavoriteImageConstructor(plant), 1, 0);

            //Title
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(TitleConstructor(titleText), 2, 0);

            int plantIndex = plants.IndexOf(plant);
           
            // add to layout
            if (plantIndex > 0 && plantIndex < plants.Count - 1)
            {
                //Previous 
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
               // gridLayout.Children.Add(PreviousImageConstructor(plants, plantIndex), 3, 0);

                //Next
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
               // gridLayout.Children.Add(NextImageConstructor(plants, plantIndex), 4, 0);
            }
            else if (plantIndex > 0)
            {
                //Previous 
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
               // gridLayout.Children.Add(PreviousImageConstructor(plants, plantIndex), 3, 0);
            }
            else if (plantIndex < plants.Count - 1)
            {
                //Next
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
              //  gridLayout.Children.Add(NextImageConstructor(plants, plantIndex), 3, 0);
            }
        
            return gridLayout;
        }

        public Image BackImageConstructor(bool cameFromSearch = false, Page searchPage = null)
        {
            Image backImage = new Image
            {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Icons.back_arrow.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };
            var backGestureRecognizer = new TapGestureRecognizer();
            backGestureRecognizer.Tapped += async (sender, e) =>
            {
                if(cameFromSearch)
                {
                    await Navigation.PushAsync(searchPage, false);
                }
                else
                {
                    GC.Collect();
                    await Navigation.PopAsync();
                }
               
            };
            backImage.GestureRecognizers.Add(backGestureRecognizer);

            return backImage;
        }

        public Image HomeImageConstructor()
        {
            Image homeImage = new Image
            {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Icons.home_icon.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };
            var homeImageGestureRecognizer = new TapGestureRecognizer();
            homeImageGestureRecognizer.Tapped += async (sender, e) =>
            {
                await Navigation.PopToRootAsync();
                await Navigation.PushAsync(new MainPage());
            };
             homeImage.GestureRecognizers.Add(homeImageGestureRecognizer);
            return homeImage;
        }

        public Label TitleConstructor(String titleText)
        {
            return new Label { Text = titleText, TextColor = Color.White, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
        }

        public Image PreviousImageConstructor(ObservableCollection<WoodyPlant> plants, int plantIndex)
        {
            Image previousImage = new Image
            {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Icons.previous_icon.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };


            if (plantIndex > 0)
            {
                WoodyPlant previousPlant = plants[plantIndex - 1];

                var previousImageGestureRecognizer = new TapGestureRecognizer();
                previousImageGestureRecognizer.Tapped += async (sender, e) =>
                {
                    previousImage.Opacity = .5;
                    await Task.Delay(200);
                    previousImage.Opacity = 1;
                    await Navigation.PushAsync(new WoodyPlantDetailPage(previousPlant, downloadImages, plants));
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                };
                previousImage.GestureRecognizers.Add(previousImageGestureRecognizer);
            }

            return previousImage;
        }

        public Image NextImageConstructor(ObservableCollection<WoodyPlant> plants, int plantIndex)
        {
            Image nextImage = new Image
            {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Icons.next_icon.png"),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };

            if (plantIndex < plants.Count - 1)
            {
                WoodyPlant nextPlant = plants[plantIndex + 1];

                var nextImageGestureRecognizer = new TapGestureRecognizer();
                nextImageGestureRecognizer.Tapped += async (sender, e) =>
                {
                    nextImage.Opacity = .5;
                    await Task.Delay(200);
                    nextImage.Opacity = 1;
                    await Navigation.PushAsync(new WoodyPlantDetailPage(nextPlant, downloadImages, plants));
                    Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                };
                nextImage.GestureRecognizers.Add(nextImageGestureRecognizer);
            }
            return nextImage;
        }
        public Image FavoriteImageConstructor(WoodyPlant plant)
        {
            string favoriteIcon = plant.isFavorite ? "favorite_icon_filled.png" : "favorite_icon_empty.png";
            string favoriteIconOpposite = !plant.isFavorite ? "favorite_icon_filled.png" : "favorite_icon_empty.png";
            Image favoriteImage = new Image
            {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Icons." + favoriteIcon),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };
            var favoriteGestureRecognizer = new TapGestureRecognizer();
            favoriteGestureRecognizer.Tapped += async (sender, e) =>
            {
                plant.isFavorite = plant.isFavorite == true ? false : true;
                favoriteImage.Source = ImageSource.FromResource("WoodyPlants.Resources.Icons." + favoriteIconOpposite);
                await App.WoodyPlantRepo.UpdatePlantAsync(plant);
            };
            favoriteImage.GestureRecognizers.Add(favoriteGestureRecognizer);

            return favoriteImage;
        }

        public async void ToPlants(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            var plantsPage = new WoodyPlantsPage(false,true,false, downloadImages);
            await Navigation.PushAsync(plantsPage);

        }
        public async void ToFavorites(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            var plantsPage = new WoodyPlantsPage(false,false,true, downloadImages);
            await Navigation.PushAsync(plantsPage);          
        }

        public async void ToSearch(object sender, EventArgs e)
        {
        
            ChangeButtonColor(sender, e);
            var plantsPage = new WoodyPlantsPage(true,false,false, downloadImages);
            await Navigation.PushAsync(plantsPage);
           
        }

        public async void ToAbout(object sender, EventArgs e)
        {
            
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("About.html", "ABOUT"));
        }

        public async void ToHowToUse(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("HowToUse.html", "HOW TO USE"));
        }

        public async void ToLink(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Links.html", "ABOUT"));
        }

        public WebView HTMLProcessor(string location)
        {
            // Generate WebView container
            var browser = new TransparentWebView();
            //var pdfBrowser = new CustomWebView { Uri = location, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            string htmlText;

            // Get file locally unless the location is a web address
            if (location.Contains("http"))
            {
                htmlText = location;
                browser.Source = htmlText;
            }
            else if (!location.Contains(".pdf"))
            {
                // Get file from PCL--in order for HTML files to be automatically pulled from the PCL, they need to be in a Views/HTML folder
                var assembly = typeof(HTMLPage).GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream("WoodyPlants.Views.HTML." + location);
                htmlText = "";
                using (var reader = new System.IO.StreamReader(stream)) { htmlText = reader.ReadToEnd(); }
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = htmlText;
                browser.Source = htmlSource;
            }
            return browser;
        }

        protected async void ChangeButtonColor(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.BackgroundColor = Color.FromHex("BBC9D845");
            await Task.Delay(100);
            button.BackgroundColor = Color.FromHex("CC1E4D2B");
        }


        protected async void ChangeDownloadText(object sender, EventArgs e)
        {
            var button = (Button)sender;
            button.Text = "Plants Downloaded";
            await Task.Delay(100);
           // button.BackgroundColor = Color.FromHex("CC1E4D2B");
        }
    }

    static class Extensions
    {
        public static void Sort<TSource, TKey>(this Collection<TSource> source, Func<TSource, TKey> keySelector, string sortDirection)
        {
            List<TSource> sortedList;
            if (sortDirection == "\u25B2")
            {
                sortedList = source.OrderByDescending(keySelector).ToList();
            }
            else
            {
                sortedList = source.OrderBy(keySelector).ToList();
            }
            source.Clear();
            foreach (var sortedItem in sortedList)
                source.Add(sortedItem);
        }
    }

}
