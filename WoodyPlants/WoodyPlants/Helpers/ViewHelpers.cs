﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using PortableApp.Models;
using PortableApp.Views;

namespace PortableApp
{
    public class TransparentWebView : WebView
    {
    }

    public class ViewHelpers : ContentPage
    {
        //
        // VIEWS
        //

        // Construct Page Container as an AbsoluteLayout with a background image
        public AbsoluteLayout ConstructPageContainer()
        {
            AbsoluteLayout pageContainer = new AbsoluteLayout { BackgroundColor = Color.Black };
            Image backgroundImage = new Image {
                Source = ImageSource.FromResource("WoodyPlants.Resources.Images.background.jpg"),
                Aspect = Aspect.AspectFill,
                Opacity = 0.7
            };
            pageContainer.Children.Add(backgroundImage, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
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
                gridLayout.Children.Add(PreviousImageConstructor(plants, plantIndex), 3, 0);

                //Next
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(NextImageConstructor(plants, plantIndex), 4, 0);
            }
            else if (plantIndex > 0)
            {
                //Previous 
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(PreviousImageConstructor(plants, plantIndex), 3, 0);
            }
            else if (plantIndex < plants.Count - 1)
            {
                //Next
                gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                gridLayout.Children.Add(NextImageConstructor(plants, plantIndex), 3, 0);
            }
        

            return gridLayout;
        }

        public Image BackImageConstructor()
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
                await Navigation.PopAsync();
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
                Source = ImageSource.FromResource("PortableApp.Resources.Icons.previous_icon.png"),
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
                    await Navigation.PushAsync(new WoodyPlantDetailPage(previousPlant, plants));
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
                Source = ImageSource.FromResource("PortableApp.Resources.Icons.next_icon.png"),
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
                    await Navigation.PushAsync(new WoodyPlantDetailPage(nextPlant, plants));
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
                Source = ImageSource.FromResource("PortableApp.Resources.Icons." + favoriteIcon),
                HeightRequest = 20,
                WidthRequest = 20,
                Margin = new Thickness(0, 15, 0, 15)
            };
            var favoriteGestureRecognizer = new TapGestureRecognizer();
            favoriteGestureRecognizer.Tapped += async (sender, e) =>
            {
                plant.isFavorite = plant.isFavorite == true ? false : true;
                favoriteImage.Source = ImageSource.FromResource("PortableApp.Resources.Icons." + favoriteIconOpposite);
                await App.WoodyPlantRepo.UpdatePlantAsync(plant);
            };
            favoriteImage.GestureRecognizers.Add(favoriteGestureRecognizer);

            return favoriteImage;
        }

        public Grid ConstructPlantsNavigationBar()
        {
            Grid gridLayout = new Grid { VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSpacing = 0 };
            gridLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

            //BACK 
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(BackImageConstructor(), 0, 0);

            // Construct filter button group
            Grid plantFilterGroup = new Grid { ColumnSpacing = -1, Margin = new Thickness(0, 8, 0, 5) };
            plantFilterGroup.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });

            // Add browse filter
            Button browseFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Browse"
            };
            //browseFilter.Clicked += FilterPlants;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(browseFilter, 0, 0);

            BoxView divider = new BoxView { HeightRequest = 40, WidthRequest = 1, BackgroundColor = Color.White };
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            plantFilterGroup.Children.Add(divider, 1, 0);

            // Add Search filter
            Button searchFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Search"
            };
            // var SearchPage = new WoodyPlantsSearchPage();
            //searchFilter.Clicked += async (s, e) => { await Navigation.PushModalAsync(SearchPage); };
            //SearchPage.InitRunSearch += HandleRunSearch;
            //SearchPage.InitCloseSearch += HandleCloseSearch;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(searchFilter, 2, 0);

            BoxView divider2 = new BoxView { HeightRequest = 40, WidthRequest = 1, BackgroundColor = Color.White };
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1) });
            plantFilterGroup.Children.Add(divider2, 3, 0);

            // Add Favorites filter
            Button favoritesFilter = new Button
            {
                Style = Application.Current.Resources["plantFilterButton"] as Style,
                Text = "Favorites"
            };
            //favoritesFilter.Clicked += FilterPlants;
            plantFilterGroup.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantFilterGroup.Children.Add(favoritesFilter, 4, 0);

            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(plantFilterGroup, 1, 0);

            //Home
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(HomeImageConstructor(), 2, 0);

            return gridLayout;
        }
        public async void ToIntroduction(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Introduction.html", "INTRODUCTION"));
        }

        public async void ToResources(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
            await Navigation.PushAsync(new HTMLPage("Resources.html", "RESOURCES"));
        }

        public async void ToWoodyPlants(object sender, EventArgs e)
        {
            ChangeButtonColor(sender, e);
           await Navigation.PushAsync(new WoodyPlantsPage());
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
    }
  
}