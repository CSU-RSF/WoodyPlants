using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WoodyPlantEcologyPage : ViewHelpers
    {
        WoodyPlant plant;
        ObservableCollection<WoodyPlant> plants;
        TransparentWebView browser;

        public WoodyPlantEcologyPage(WoodyPlant plant, ObservableCollection<WoodyPlant> plants)
        {
            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container

            Grid navigationBar = ConstructPlantNavigationBar(plant.scientificnameweber, plant, plants);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            ScrollView contentScrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            //TransparentWebView browser = ConstructHTMLContent(plant);
            //browser.Navigating += ToWoodyType;

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            //var WoodyTypes = ConstructWoodyTypes(plant.ecologicalsystems);
            //innerContainer.RowDefinitions.Add(new RowDefinition { });
            //innerContainer.Children.Add(WoodyTypes, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;

        }
        
        //public TransparentWebView ConstructHTMLContent(WoodyPlant plant)
        //{
        //    browser = new TransparentWebView();
        //    var htmlSource = new HtmlWebViewSource();
        //    string html = "";

        //    html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
        //    html += "<style>body, a { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; } .iconImg { height: 40px; }</style>";

        //    html += "<div class='section_header'>HABITAT & ECOLOGY</div>" + plant.habitat;

        //    html += "<div class='section_header'>COMMENTS</div>" + plant.comments;

        //    html += "<div class='section_header'>Woody TYPES</div>" + ReconstructWoodyTypes(plant.ecologicalsystems);

        //    html += "<div class='section_header'>ANIMAL USE</div>" + plant.animaluse.Replace("resources/images/animals/", "");

        //    html += "</body></html>";

        //    htmlSource.Html = html;
        //    browser.Source = htmlSource;
            
        //    return browser;
        //}

        //// Prepare Woody type for navigation on click
        //private async void ToWoodyType(object sender, WebNavigatingEventArgs e)
        //{
        //    string[] urlArray = e.Url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        //    string title = urlArray[urlArray.Length - 1];
        //    if (!title.Contains("app"))
        //    {
        //        WoodyType WoodyType = new WoodyType { Title = title, Description = "WoodyType-" + title.Replace(" ", "").Replace("%20", "") + ".html" };
        //        var detailPage = new WoodyTypesDetailPage(WoodyType);
        //        await Navigation.PushModalAsync(detailPage);
        //    }            
        //}
        
        // Reconstruct Woody types ('ecologicalsystems' field) so as to contain an internal link
        private string ReconstructWoodyTypes(string WoodyTypes)
        {
            List<string> allWoodysArray = new List<string> { "Marsh", "Wet Meadow", "Mesic Meadow", "Fen", "Playa", "Subalpine Riparian Woodland", "Subalpine Riparian Shrubland", "Foothills Riparian", "Plains Riparian", "Plains Floodplain", "Greasewood Flats", "Hanging Garden" };
            foreach (string item in allWoodysArray)
            {
                if (WoodyTypes.Contains(item))
                {
                    string replacementHTML = "<a href='" + item + "'>" + item + "</a>";
                    WoodyTypes = WoodyTypes.Replace(item, replacementHTML);
                }
            }
            return WoodyTypes;
        }
    }
}
