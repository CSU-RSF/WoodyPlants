using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WoodyPlants
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
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(backImage, 0, 0);

            //Title
  
            Label title = new Label { Text = titleText, TextColor = Color.White, FontSize = 16, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(6, GridUnitType.Star) });
            gridLayout.Children.Add(title, 1, 0);

            //Home
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
            gridLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridLayout.Children.Add(homeImage, 2, 0);

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
           // await Navigation.PushAsync(new WoodyPlantsPage());
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
