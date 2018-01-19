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

            Grid navigationBar = ConstructPlantNavigationBar(plant.scientificNameWeber, plant, plants);
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });
            innerContainer.Children.Add(navigationBar, 0, 0);

            ScrollView contentScrollView = new ScrollView
            {
                BackgroundColor = Color.FromHex("88000000"),
                Padding = new Thickness(20, 5, 20, 5),
                Margin = new Thickness(0, 0, 0, 0)
            };

            TransparentWebView browser = ConstructHTMLContent(plant);
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

            base.OnAppearing();

        }

        public TransparentWebView ConstructHTMLContent(WoodyPlant plant)
        {
            browser = new TransparentWebView();
            var htmlSource = new HtmlWebViewSource();
            string html = "";

            html += "<!DOCTYPE html><html lang='en' xmlns='http://www.w3.org/1999/xhtml'><head><meta charset = 'utf-8' /><title>Plant Info Page</title></head><body>";
            html += "<style>body, a { color: white; font-size: 0.9em; } .section_header { font-weight: bold; border-bottom: 1px solid white; margin: 10px 0; } .embedded_table { width: 100%; margin-left: 10px; } .iconImg { height: 40px; }</style>";

            if ((plant.frequency != null && plant.frequency.Length != 0) || (plant.lifeZone != null && plant.lifeZone.Length != 0) || (plant.habitat != null && plant.habitat.Length != 0) || (plant.endemicLocation != null && plant.endemicLocation.Length != 0))
            {
                html += "<div class='section_header'>ECOSYSTEM</div>";
                if (plant.frequency != null && plant.frequency.Length != 0)
                {
                    html += "<strong>Frequency: </strong>" + plant.frequency + "<br/>";
                }
                if (plant.lifeZone != null && plant.lifeZone.Length != 0)
                {
                    html += "<strong>Life Zone: </strong>" + plant.lifeZone + "<br/>";
                }
                if (plant.habitat != null && plant.habitat.Length != 0)
                {
                    html += "<strong>Habitat: </strong>" + plant.habitat + "<br/>";
                }
                if (plant.endemicLocation != null && plant.endemicLocation.Length != 0)
                {
                    html += "<strong>Endemic Location: </strong>" + plant.endemicLocation + "<br/>";
                }       
            }

            if ((plant.edibility != null && plant.edibility.Length != 0) || (plant.toxicity != null && plant.toxicity.Length != 0) || (plant.fiber != null && plant.fiber.Length != 0) || (plant.otherUses != null && plant.otherUses.Length != 0))
            {
                html += "<div class='section_header'>HUMAN CONNECTIONS</div>";
                if (plant.edibility != null && plant.edibility.Length != 0)
                {
                    html += "<strong>Edibility: </strong>" + plant.edibility + "<br/>";
                }
                if (plant.toxicity != null && plant.toxicity.Length != 0)
                {
                    html += "<strong>Toxicity: </strong>" + plant.toxicity + "<br/>";
                }
                if (plant.fiber != null && plant.fiber.Length != 0)
                {
                    html += "<strong>Fiber/Dye: </strong>" + plant.fiber + "<br/>";
                }
                if (plant.otherUses != null && plant.otherUses.Length != 0)
                {
                    html += "<strong>Other Uses: </strong>" + plant.otherUses + "<br/>";
                }   
            }

            if ((plant.alien != null && plant.alien.Length != 0) || (plant.weedManagement != null && plant.weedManagement.Length != 0))
            { 
                html += "<div class='section_header'>WEED</div>";
                if (plant.alien != null && plant.alien.Length != 0)
                {
                    html += "<strong>Alien: </strong>" + plant.alien + "<br/>";
                }
                if (plant.weedManagement != null && plant.weedManagement.Length != 0)
                {
                    html += "<strong>Weed Management: </strong>" + plant.weedManagement + "<br/>";
                }
            }

            if ((plant.landscapingUse != null && plant.landscapingUse.Length != 0) || (plant.matureHeight != null && plant.matureHeight.Length != 0) || (plant.matureSpread != null && plant.matureSpread.Length != 0) || (plant.siteRequirements != null && plant.siteRequirements.Length != 0) || (plant.soilRequirements != null && plant.soilRequirements.Length != 0) || (plant.moistureRequirements != null && plant.moistureRequirements.Length != 0) || (plant.cultivar != null && plant.cultivar.Length != 0) || (plant.availability != null && plant.availability.Length != 0))
            {
                html += "<div class='section_header'>LANDSCAPING</div>";
                if (plant.landscapingUse != null && plant.landscapingUse.Length != 0)
                {
                    html += "<strong>Landscaping Use: </strong>" + plant.landscapingUse + "<br/>";
                }
                if (plant.matureHeight != null && plant.matureHeight.Length != 0)
                {
                    html += "<strong>Mature Height: </strong>" + plant.matureHeight + "<br/>";
                }
                if (plant.matureSpread != null && plant.matureSpread.Length != 0)
                {
                    html += "<strong>Mature Spread: </strong>" + plant.matureSpread + "<br/>";
                }
                if (plant.siteRequirements != null && plant.siteRequirements.Length != 0)
                {
                    html += "<strong>Site Requirements: </strong>" + plant.siteRequirements + "<br/>";
                }
                if (plant.soilRequirements != null && plant.soilRequirements.Length != 0)
                {
                    html += "<strong>Soil Requirements: </strong>" + plant.soilRequirements + "<br/>";
                }
                if (plant.moistureRequirements != null && plant.moistureRequirements.Length != 0)
                {
                    html += "<strong>Moisture Requirements: </strong>" + plant.moistureRequirements + "<br/>";
                }
                if (plant.cultivar != null && plant.cultivar.Length != 0)
                {
                    html += "<strong>Cultivar: </strong>" + plant.cultivar + "<br/>";
                }
                if (plant.availability != null && plant.availability.Length != 0)
                {
                    html += "<strong>Availability: </strong>" + plant.availability + "<br/>";
                }    
            }

            if ((plant.ecologicalRelationships != null && plant.ecologicalRelationships.Length != 0) || (plant.scientificNameMeaning != null && plant.scientificNameMeaning.Length != 0) || (plant.derivation != null && plant.derivation.Length != 0) || (plant.comments != null && plant.comments.Length != 0))
            {
                html += "<div class='section_header'>NOTES</div>";
                if (plant.ecologicalRelationships != null && plant.ecologicalRelationships.Length != 0)
                {
                    html += "<strong>Ecological Relationships: </strong>" + plant.ecologicalRelationships + "<br/>";
                }
                if (plant.scientificNameMeaning != null && plant.scientificNameMeaning.Length != 0)
                {
                    html += "<strong>Scientific Name Meaning: </strong>" + plant.scientificNameMeaning + "<br/>";
                }
                if (plant.derivation != null && plant.derivation.Length != 0)
                {
                    html += "<strong>Derivation: </strong>" + plant.derivation + "<br/>";
                }
                if (plant.comments != null && plant.comments.Length != 0)
                {
                    html += "<strong>Comments: </strong>" + plant.comments + "<br/>";
                }  
            }
            html += "</body></html>";

            htmlSource.Html = html;
            browser.Source = htmlSource;

            return browser;
        }

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
