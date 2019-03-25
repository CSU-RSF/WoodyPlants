using PortableApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp
{
    public partial class WoodyPlantInfoPage : ViewHelpers
    {

        public WoodyPlantInfoPage(WoodyPlant plant, ObservableCollection<WoodyPlant> plants)
        {

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid { Padding = new Thickness(0, Device.OnPlatform(10, 0, 0), 0, 0) };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Add header to inner container
            Grid navigationBar = ConstructPlantNavigationBar(plant.commonName, plant, plants);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);
            
            ScrollView contentScrollView = new ScrollView {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            TransparentWebView browser = ConstructHTMLContent(plant);

            contentScrollView.Content = browser;
            innerContainer.RowDefinitions.Add(new RowDefinition { });
            innerContainer.Children.Add(contentScrollView, 0, 1);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);
            Content = pageContainer;
        }

        public TransparentWebView ConstructHTMLContent(WoodyPlant plant)
        {
            var browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";

            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body { color: white; font-size: 0.9em; padding-bottom: 200px; padding-top: 50px; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; }</style>";

            html += "<div class='section_header'>NAME</div>";
            if (plant.commonName != null && plant.commonName.Length != 0)
            {
                html += "<strong>Common Name: </strong>" + plant.commonName + "<br/>";
            }
            if (plant.scientificNameWeber != null && plant.scientificNameWeber.Length != 0)
            {
                html += "<strong>Scientific Name: </strong>" + plant.scientificNameWeber + "<br/>";
            }
            if (plant.scientificNameOther != null && plant.scientificNameOther.Length != 0)
            {
                html += "<strong>Synonyms: </strong>" + plant.scientificNameOther + "<br/>";
            }
            if (plant.family != null && plant.family.Length != 0)
            {
                html += "<strong>Family: </strong>" + plant.family + "<br/>";
            }
            if (plant.keyCharacteristics != null && plant.keyCharacteristics.Length != 0)
            {
                html += "<div class='section_header'>KEY CHARACTERISTICS</div>";
                html += plant.keyCharacteristics;
            }

            if (plant.flowerDescription != null && plant.flowerDescription.Length != 0)
            {
                html += "<div class='section_header'>FLOWER</div>";
                html += plant.flowerDescription;
            }

            if (plant.seasonOfBloom != null && plant.seasonOfBloom.Length !=0)
            { 
                html += "<div class='section_header'>SEASON OF BLOOM</div>";
                html += plant.seasonOfBloom;
            }

            if (plant.fruitDescription != null && plant.fruitDescription.Length != 0)
            {
                html += "<div class='section_header'>FRUIT/CONE DESCRIPTION</div>";
                html += plant.fruitDescription;
            }

            string leafDescrip = "";
            if (plant.leafType != null && plant.leafType.Length != 0)
            {
                leafDescrip += "<b>Leaf Type: </b>";
                leafDescrip += plant.leafType;
                leafDescrip += "</br>";
            }
            if (plant.leafArrangement != null && plant.leafArrangement.Length != 0)
            {
                leafDescrip += "<b>Leaf Arrangement: </b>";
                leafDescrip += plant.leafArrangement;
            }

            if (!leafDescrip.Equals("")) {
                html += "<div class='section_header'>LEAF DESCRIPTION</div>";
                html += leafDescrip;
            }

            if (plant.barkDescription != null && plant.barkDescription.Length != 0)
            {
                html += "<div class='section_header'>BARK/STEM DESCRIPTION</div>";
                html += plant.barkDescription;
            }

            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;
            return browser;
        }

    }
}
