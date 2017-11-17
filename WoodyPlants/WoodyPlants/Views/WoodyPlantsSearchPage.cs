using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public partial class WoodyPlantsSearchPage : ViewHelpers
    {
        public EventHandler InitRunSearch;
        public EventHandler InitCloseSearch;

        public ObservableCollection<WoodyPlant> plants;
        public ObservableCollection<WoodySearch> searchCriteriaDB;
        public ObservableCollection<SearchCharacteristicIcon> searchCriteria;

        StackLayout leafTypesLayout;
        List<SearchCharacteristicIcon> leafTypes;
        Button searchButton;

        public WoodyPlantsSearchPage()
        {
            plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepo.GetAllWoodyPlants());
            searchCriteriaDB = new ObservableCollection<WoodySearch>(App.WoodySearchRepo.GetAllWoodySearchCriteria());
            searchCriteria = SearchCharacteristicIconsCollection();

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            Grid innerContainer = new Grid {
                Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("88000000"),
                RowSpacing = 10
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            StackLayout searchFilters = new StackLayout { Spacing = 10 };

     

            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            StackLayout flowerColorLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            flowerColorLayout.Children.Add(yellowFlowerColor);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            flowerColorLayout.Children.Add(blueFlowerColor);

            SearchCharacteristicIcon redFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Red");
            flowerColorLayout.Children.Add(redFlowerColor);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            flowerColorLayout.Children.Add(brownFlowerColor);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            flowerColorLayout.Children.Add(whiteFlowerColor);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
            flowerColorLayout.Children.Add(greenFlowerColor);

            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
            flowerColorLayout.Children.Add(orangeFlowerColor);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            flowerColorLayout.Children.Add(pinkFlowerColor);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            flowerColorLayout.Children.Add(purpleFlowerColor);

            searchFilters.Children.Add(flowerColorLayout);

            // Add Type of Plant
            Label  barkTextureLabel = new Label { Text = "Bark Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(barkTextureLabel);

            StackLayout barkTextureLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon barkTextureSmooth = searchCriteria.First(x => x.Characteristic == "BarkTexture-Smooth");
            barkTextureLayout.Children.Add(barkTextureSmooth);

            SearchCharacteristicIcon barkTextureBumpy = searchCriteria.First(x => x.Characteristic == "BarkTexture-Bumpy");
            barkTextureLayout.Children.Add(barkTextureBumpy);

            SearchCharacteristicIcon barkTexturePeeling = searchCriteria.First(x => x.Characteristic == "BarkTexture-Peeling");
            barkTextureLayout.Children.Add(barkTexturePeeling);

            SearchCharacteristicIcon barkTextureFurrowed = searchCriteria.First(x => x.Characteristic == "BarkTexture-Furrowed");
            barkTextureLayout.Children.Add(barkTextureFurrowed);

            searchFilters.Children.Add(barkTextureLayout);

            // Add Type of Plant
            Label flowerClusterLabel = new Label { Text = "Flower Cluster:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerClusterLabel);

            StackLayout flowerClusterLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerClusterDense = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Dense");
            flowerClusterLayout.Children.Add(flowerClusterDense);

            SearchCharacteristicIcon flowerClusterLoose = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Loose");
            flowerClusterLayout.Children.Add(flowerClusterLoose);

            SearchCharacteristicIcon flowerClusterSolitary = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Solitary");
            flowerClusterLayout.Children.Add(flowerClusterSolitary);

            SearchCharacteristicIcon flowerClusterCatkin = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Catkin");
            flowerClusterLayout.Children.Add(flowerClusterCatkin);

            searchFilters.Children.Add(flowerClusterLayout);

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);

            StackLayout flowerShapeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerShapeInconspicuous = searchCriteria.First(x => x.Characteristic == "FlowerShape-Inconspicuous");
            flowerShapeLayout.Children.Add(flowerShapeInconspicuous);

            SearchCharacteristicIcon flowerShapeRound = searchCriteria.First(x => x.Characteristic == "FlowerShape-Round");
            flowerShapeLayout.Children.Add(flowerShapeRound);

            SearchCharacteristicIcon flowerShapeBellShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-BellShaped");
            flowerShapeLayout.Children.Add(flowerShapeBellShaped);

            SearchCharacteristicIcon flowerShapeCupShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-CupShaped");
            flowerShapeLayout.Children.Add(flowerShapeCupShaped);

            SearchCharacteristicIcon flowerShapeStarShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-StarShaped");
            flowerShapeLayout.Children.Add(flowerShapeStarShaped);

            SearchCharacteristicIcon flowerShapeOther = searchCriteria.First(x => x.Characteristic == "FlowerShape-Other");
            flowerShapeLayout.Children.Add(flowerShapeOther);

            searchFilters.Children.Add(flowerShapeLayout);

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitTypeLabel);

            StackLayout fruitTypeLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon fruitTypeDrySeed = searchCriteria.First(x => x.Characteristic == "FruitType-DrySeed");
            fruitTypeLayout.Children.Add(fruitTypeDrySeed);

            SearchCharacteristicIcon fruitTypeAcorn = searchCriteria.First(x => x.Characteristic == "FruitType-Acorn");
            fruitTypeLayout.Children.Add(fruitTypeAcorn);

            SearchCharacteristicIcon fruitTypeFleshy = searchCriteria.First(x => x.Characteristic == "FruitType-Fleshy");
            fruitTypeLayout.Children.Add(fruitTypeFleshy);

            SearchCharacteristicIcon fruitTypeCone = searchCriteria.First(x => x.Characteristic == "FruitType-Cone");
            fruitTypeLayout.Children.Add(fruitTypeCone);

            SearchCharacteristicIcon fruitTypeCapsule = searchCriteria.First(x => x.Characteristic == "FruitType-Capsule");
            fruitTypeLayout.Children.Add(fruitTypeCapsule);

            SearchCharacteristicIcon fruitTypeSamara = searchCriteria.First(x => x.Characteristic == "FruitType-Samara");
            fruitTypeLayout.Children.Add(fruitTypeSamara);

            searchFilters.Children.Add(fruitTypeLayout);

            
            // Add Type of Plant
            Label fruitColorLabel = new Label { Text = "Fruit Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitColorLabel);

            StackLayout fruitColorLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Yellow");
            fruitColorLayout.Children.Add(yellowFruitColor);

            SearchCharacteristicIcon blueFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Blue");
            fruitColorLayout.Children.Add(blueFruitColor);

            SearchCharacteristicIcon redFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Red");
            fruitColorLayout.Children.Add(redFruitColor);

            SearchCharacteristicIcon brownFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Brown");
            fruitColorLayout.Children.Add(brownFruitColor);

            SearchCharacteristicIcon whiteFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-White");
            fruitColorLayout.Children.Add(whiteFruitColor);

            SearchCharacteristicIcon greenFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Green");
            fruitColorLayout.Children.Add(greenFruitColor);

            SearchCharacteristicIcon orangeFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Orange");
            fruitColorLayout.Children.Add(orangeFruitColor);

            SearchCharacteristicIcon blackFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Black");
            fruitColorLayout.Children.Add(blackFruitColor);

            SearchCharacteristicIcon purpleFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Purple");
            fruitColorLayout.Children.Add(purpleFruitColor);

            SearchCharacteristicIcon grayFruitColor = searchCriteria.First(x => x.Characteristic == "FruitColor-Gray");
            fruitColorLayout.Children.Add(grayFruitColor);

            searchFilters.Children.Add(fruitColorLayout);


            ScrollView scrollView = new ScrollView()
            {
                Content = searchFilters,
                Orientation = ScrollOrientation.Both,
            };

            innerContainer.Children.Add(scrollView, 0, 0);


            // Add Search/Reset button group
            Grid searchButtons = new Grid();
            searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button resetButton = new Button { Text = "RESET", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            resetButton.Clicked += ResetSearchFilters;
            searchButtons.Children.Add(resetButton, 0, 0);

            searchButton = new Button { Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
            searchButton.Clicked += RunSearch;
            searchButtons.Children.Add(searchButton, 1, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(searchButtons, 0, 1);

            // Add Close button
            Button closeButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CLOSE",
                BorderRadius = Device.OnPlatform(0, 1, 0)
            };
            closeButton.Clicked += CloseSearch;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });
            innerContainer.Children.Add(closeButton, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            Content = pageContainer;

          
        }

        private async void ResetSearchFilters(object sender, EventArgs e)
        {
            foreach(var searchCrit in searchCriteria)
            {
                searchCrit.BorderWidth = 0;
                searchCrit.Query = false;
                WoodySearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == searchCrit.Characteristic);
                correspondingDBRecord.Query = false;
                await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);
            }
            plants = await App.WoodyPlantRepo.GetAllWoodyPlantsAsync();
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private async void ProcessSearchFilter(object sender, EventArgs e)
        {
            // Update record in database and add or remove border
            SearchCharacteristicIcon button = (SearchCharacteristicIcon)sender;
            WoodySearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == button.Characteristic);
            if (button.Query == true)
            {
                correspondingDBRecord.Query = button.Query = false;
                button.BorderWidth = 0;
            }
            else if (button.Query == false)
            {
                correspondingDBRecord.Query = button.Query = true;
                button.BorderWidth = 1;
            }
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);

            plants = await App.WoodyPlantRepo.FilterPlantsBySearchCriteria();
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private void RunSearch(object sender, EventArgs e)
        {
            InitRunSearch?.Invoke(this, EventArgs.Empty);
        }

        private void CloseSearch(object sender, EventArgs e)
        {
            InitCloseSearch?.Invoke(this, EventArgs.Empty);
        }

        private ObservableCollection<SearchCharacteristicIcon> SearchCharacteristicIconsCollection()
        {
            searchCriteria = new ObservableCollection<SearchCharacteristicIcon>();
            foreach (WoodySearch searchItem in searchCriteriaDB)
            {
                SearchCharacteristicIcon item = new SearchCharacteristicIcon();
                item.BindingContext = searchItem;
                item.SetBinding(SearchCharacteristicIcon.CharacteristicProperty, new Binding("Characteristic"));
                item.SetBinding(SearchCharacteristicIcon.TextProperty, new Binding("Name"));
                item.SetBinding(SearchCharacteristicIcon.ImageProperty, new Binding("IconFileName"));
                item.SetBinding(SearchCharacteristicIcon.QueryProperty, new Binding("Query"));
                item.SetBinding(SearchCharacteristicIcon.Column1Property, new Binding("Column1"));
                item.SetBinding(SearchCharacteristicIcon.SearchString1Property, new Binding("SearchString1"));
                item.Clicked += ProcessSearchFilter;
                item.BorderWidth = item.Query ? 1 : 0;
                searchCriteria.Add(item);
            }            
            return searchCriteria;
        }

    }
}
