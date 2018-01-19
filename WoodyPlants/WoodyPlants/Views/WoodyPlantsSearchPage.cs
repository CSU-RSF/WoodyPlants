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
        StackLayout searchFilters;

        SearchCharacteristicIcon deciduousPlantType;
        SearchCharacteristicIcon coniferPlantType;
        SearchCharacteristicIcon vinePlantType;
        SearchCharacteristicIcon cactiPlantType;

        public WoodyPlantsSearchPage()
        {
            plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());
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

            searchFilters = new StackLayout { Spacing = 10 };

            // Add Type of Plant
            Label plantTypeLabel = new Label { Text = "Plant Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantTypeLabel);

            WrapLayout plantTypeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            deciduousPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Deciduous");
            //deciduousPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(deciduousPlantType);

            coniferPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Conifer");
            //coniferPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(coniferPlantType);

            vinePlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Vine");
            //vinePlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(vinePlantType);

            cactiPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Cacti");
            //cactiPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(cactiPlantType);

            searchFilters.Children.Add(plantTypeLayout);




            ScrollView scrollView = new ScrollView()
            {
                Content = searchFilters,
                Orientation = ScrollOrientation.Vertical,
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
            foreach (var searchCrit in searchCriteria)
            {
                searchCrit.BorderWidth = 0;
                searchCrit.Query = false;
                WoodySearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == searchCrit.Characteristic);
                correspondingDBRecord.Query = false;
                await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);
            }

            int count = searchFilters.Children.Count();

            for (int i = 2; i < count;)
            {
                searchFilters.Children.RemoveAt(i);
                count--;
            }

            plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
        }

        private async void ResetTypeButtons(object sender, EventArgs e)
        {
            WoodySearch correspondingDBRecord1 = searchCriteriaDB.First(x => x.Characteristic == deciduousPlantType.Characteristic);
            WoodySearch correspondingDBRecord2 = searchCriteriaDB.First(x => x.Characteristic == coniferPlantType.Characteristic);
            WoodySearch correspondingDBRecord3 = searchCriteriaDB.First(x => x.Characteristic == vinePlantType.Characteristic);
            WoodySearch correspondingDBRecord4 = searchCriteriaDB.First(x => x.Characteristic == cactiPlantType.Characteristic);

            deciduousPlantType.BorderWidth = 0;
            coniferPlantType.BorderWidth = 0;
            vinePlantType.BorderWidth = 0;
            cactiPlantType.BorderWidth = 0;

            correspondingDBRecord1.Query = deciduousPlantType.Query = false;
            correspondingDBRecord2.Query = deciduousPlantType.Query = false;
            correspondingDBRecord3.Query = deciduousPlantType.Query = false;
            correspondingDBRecord4.Query = deciduousPlantType.Query = false;

            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord1);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord2);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord3);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord4);
            
        }


        private async void ChangeSearchCharacteristics(object sender, EventArgs e)
        {
            //MightHaveTo Delete
            //ResetSearchFilters(sender, e);

            plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());

            int count = searchFilters.Children.Count();

            for (int i = 2; i < count;)
            {
                searchFilters.Children.RemoveAt(i);
                count--;
            }

            SearchCharacteristicIcon button = (SearchCharacteristicIcon)sender;
            //WoodySearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == button.Characteristic);
            if (button.Characteristic.Equals("PlantType-Deciduous"))
            {
                    LeafShapeSearch();
                    LeafArrangementSearch();
                    TwigTextureSearch();
                    BarkTextureSearch();
                    FruitTypeSearch();
                    FlowerShapeSearch();
                    FlowerColorSearch();

            }
            else if (button.Characteristic.Equals("PlantType-Conifer"))
            {
                    NeedleShapeSearch();
                    ConeTypeSearch();
                    //FruitColorSearch();

            }
            else if (button.Characteristic.Equals("PlantType-Vine"))
            {
                    VineLeafShapeSearch();
                    VineLeafArrangementSearch();
                    VineFruitTypeSearch();
                    VineFlowerShapeSearch();
                    VineFlowerColorSearch();


            }
            else if (button.Characteristic.Equals("PlantType-Cacti"))
            {
                    CactusShapeSearch();
                    CactusFlowerColorSearch();

            }
        }

        private async void ProcessSearchFilter(object sender, EventArgs e)
        {
            // Update record in database and add or remove border
            SearchCharacteristicIcon button = (SearchCharacteristicIcon)sender;
            WoodySearch correspondingDBRecord = searchCriteriaDB.First(x => x.Characteristic == button.Characteristic);

            if (!button.Characteristic.Equals("PlantType-Deciduous") && !button.Characteristic.Equals("PlantType-Cacti") && !button.Characteristic.Equals("PlantType-Vine") && !button.Characteristic.Equals("PlantType-Conifer"))
            {
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
            }
            else
            {
                if (button.Query == true)
                {
                    ResetSearchFilters(sender,e);
                    correspondingDBRecord.Query = button.Query = false;
                }
                else if (button.Query == false)
                {
                    ResetTypeButtons(sender, e);
                    //ResetSearchFilters(sender, e);
                    button.BorderWidth = 1;
                    correspondingDBRecord.Query = button.Query = true;
                    ChangeSearchCharacteristics(sender, e);
                }
            }
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord);

            plants = await App.WoodyPlantRepoLocal.FilterPlantsBySearchCriteria();
            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
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

        private void LeafShapeSearch()
        {
            Label leafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafShapeLabel);

            WrapLayout leafShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon narrowLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Narrow");
            leafShapeLayout.Children.Add(narrowLeafShape);

            SearchCharacteristicIcon deltoidLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Deltoid");
            leafShapeLayout.Children.Add(deltoidLeafShape);

            SearchCharacteristicIcon orbicularLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Orbicular");
            leafShapeLayout.Children.Add(orbicularLeafShape);

            SearchCharacteristicIcon oblanceolateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Oblanceolate");
            leafShapeLayout.Children.Add(oblanceolateLeafShape);

            SearchCharacteristicIcon palmatelyLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Palmately");
            leafShapeLayout.Children.Add(palmatelyLeafShape);

            SearchCharacteristicIcon lobedLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Lobed");
            leafShapeLayout.Children.Add(lobedLeafShape);

            SearchCharacteristicIcon pinnateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Pinnate");
            leafShapeLayout.Children.Add(pinnateLeafShape);

            searchFilters.Children.Add(leafShapeLayout);
        }

        private void VineLeafShapeSearch()
        {
            Label vineLeafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(vineLeafShapeLabel);

            WrapLayout vineLeafShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon oneLeafShape = searchCriteria.First(x => x.Characteristic == "ShapeVineLeaf-Shape1");
            vineLeafShapeLayout.Children.Add(oneLeafShape);

            SearchCharacteristicIcon twoLeafShape = searchCriteria.First(x => x.Characteristic == "ShapeVineLeaf-Shape2");
            vineLeafShapeLayout.Children.Add(twoLeafShape);

            searchFilters.Children.Add(vineLeafShapeLayout);
        }

        private void NeedleShapeSearch()
        {
            Label needleShapeLabel = new Label { Text = "Needle Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(needleShapeLabel);

            WrapLayout needleLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon twoClusterShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-TwoCluster");
            needleLayout.Children.Add(twoClusterShape);

            SearchCharacteristicIcon fiveClusterShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-FiveCluster");
            needleLayout.Children.Add(fiveClusterShape);

            SearchCharacteristicIcon flatShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Flat");
            needleLayout.Children.Add(flatShape);

            SearchCharacteristicIcon sharpShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Sharp");
            needleLayout.Children.Add(sharpShape);

            SearchCharacteristicIcon scaleShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Scale");
            needleLayout.Children.Add(scaleShape);

            searchFilters.Children.Add(needleLayout);
        }

        private void LeafArrangementSearch()
        {
            // Add Type of Plant
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            WrapLayout leafArrangementLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon alternateLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Alternate");
            leafArrangementLayout.Children.Add(alternateLeafArrangement);

            SearchCharacteristicIcon oppositeLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Opposite");
            leafArrangementLayout.Children.Add(oppositeLeafArrangement);

            SearchCharacteristicIcon whorledLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Whorled");
            leafArrangementLayout.Children.Add(whorledLeafArrangement);

            SearchCharacteristicIcon basalLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Basal");
            leafArrangementLayout.Children.Add(basalLeafArrangement);

            searchFilters.Children.Add(leafArrangementLayout);
        }

        private void VineLeafArrangementSearch()
        {
            // Add Type of Plant
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            WrapLayout leafArrangementLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon alternateLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Alternate");
            leafArrangementLayout.Children.Add(alternateLeafArrangement);

            SearchCharacteristicIcon oppositeLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Opposite");
            leafArrangementLayout.Children.Add(oppositeLeafArrangement);

            SearchCharacteristicIcon whorledLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Whorled");
            leafArrangementLayout.Children.Add(whorledLeafArrangement);

            searchFilters.Children.Add(leafArrangementLayout);
        }

        private void TwigTextureSearch()
        {
            // Add Type of Plant
            Label twigTextureLabel = new Label { Text = "Twig Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(twigTextureLabel);

            WrapLayout twigTextureLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon hairyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Hairy");
            twigTextureLayout.Children.Add(hairyTwigTexture);

            SearchCharacteristicIcon smoothTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Smooth");
            twigTextureLayout.Children.Add(smoothTwigTexture);

            SearchCharacteristicIcon roughTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Rough");
            twigTextureLayout.Children.Add(roughTwigTexture);

            SearchCharacteristicIcon peelingTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Peeling");
            twigTextureLayout.Children.Add(peelingTwigTexture);

            SearchCharacteristicIcon thornyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Thorny");
            twigTextureLayout.Children.Add(thornyTwigTexture);

            SearchCharacteristicIcon stickyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Sticky");
            twigTextureLayout.Children.Add(stickyTwigTexture);

            searchFilters.Children.Add(twigTextureLayout);
        }

        private void BarkTextureSearch()
        {
            // Add Type of Plant
            Label barkTextureLabel = new Label { Text = "Bark Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(barkTextureLabel);

            WrapLayout barkTextureLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon barkTextureSmooth = searchCriteria.First(x => x.Characteristic == "BarkTexture-Smooth");
            barkTextureLayout.Children.Add(barkTextureSmooth);

            SearchCharacteristicIcon barkTextureBumpy = searchCriteria.First(x => x.Characteristic == "BarkTexture-Bumpy");
            barkTextureLayout.Children.Add(barkTextureBumpy);

            SearchCharacteristicIcon barkTexturePeeling = searchCriteria.First(x => x.Characteristic == "BarkTexture-Peeling");
            barkTextureLayout.Children.Add(barkTexturePeeling);

            SearchCharacteristicIcon barkTextureFurrowed = searchCriteria.First(x => x.Characteristic == "BarkTexture-Furrowed");
            barkTextureLayout.Children.Add(barkTextureFurrowed);

            searchFilters.Children.Add(barkTextureLayout);
        }

        private void FlowerCluserSearch()
        {
            // Add Type of Plant
            Label flowerClusterLabel = new Label { Text = "Flower Cluster:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerClusterLabel);

            WrapLayout flowerClusterLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerClusterDense = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Dense");
            flowerClusterLayout.Children.Add(flowerClusterDense);

            SearchCharacteristicIcon flowerClusterLoose = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Loose");
            flowerClusterLayout.Children.Add(flowerClusterLoose);

            SearchCharacteristicIcon flowerClusterSolitary = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Solitary");
            flowerClusterLayout.Children.Add(flowerClusterSolitary);

            SearchCharacteristicIcon flowerClusterCatkin = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Catkin");
            flowerClusterLayout.Children.Add(flowerClusterCatkin);

            searchFilters.Children.Add(flowerClusterLayout);
        }

        private void FlowerShapeSearch()
        {
            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);

            WrapLayout flowerShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

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
        }

        private void VineFlowerShapeSearch()
        {
            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);

            WrapLayout flowerShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flowerShapeInconspicuous = searchCriteria.First(x => x.Characteristic == "FlowerVineShape-Shape1");
            flowerShapeLayout.Children.Add(flowerShapeInconspicuous);

            SearchCharacteristicIcon flowerShapeRound = searchCriteria.First(x => x.Characteristic == "FlowerVineShape-Shape2");
            flowerShapeLayout.Children.Add(flowerShapeRound);

            SearchCharacteristicIcon flowerShapeBellShaped = searchCriteria.First(x => x.Characteristic == "FlowerVineShape-Shape3");
            flowerShapeLayout.Children.Add(flowerShapeBellShaped);           

            searchFilters.Children.Add(flowerShapeLayout);
        }

        private void FruitTypeSearch()
        {
            
                // Add Type of PlantFIXXXXXXXXXXXXXX
                Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
                searchFilters.Children.Add(fruitTypeLabel);

                WrapLayout fruitTypeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

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

        }

        private void VineFruitTypeSearch()
        {

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitTypeLabel);

            WrapLayout fruitTypeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };          

            SearchCharacteristicIcon fruitTypeFleshy = searchCriteria.First(x => x.Characteristic == "FruitType-Fleshy");
            fruitTypeLayout.Children.Add(fruitTypeFleshy);
        
            SearchCharacteristicIcon fruitTypeCapsule = searchCriteria.First(x => x.Characteristic == "FruitType-Capsule");
            fruitTypeLayout.Children.Add(fruitTypeCapsule);
     
            searchFilters.Children.Add(fruitTypeLayout);

        }

        private void ConeTypeSearch()
        {

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label coneTypeLabel = new Label { Text = "Cone Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(coneTypeLabel);

            WrapLayout coneTypeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon coneTypeCone = searchCriteria.First(x => x.Characteristic == "ConeType-SolidCone");
            coneTypeLayout.Children.Add(coneTypeCone);

            SearchCharacteristicIcon conePapTypeCone = searchCriteria.First(x => x.Characteristic == "ConeType-PaperyCone");
            coneTypeLayout.Children.Add(conePapTypeCone);

            SearchCharacteristicIcon coneTypeBerry = searchCriteria.First(x => x.Characteristic == "ConeType-Berry");
            coneTypeLayout.Children.Add(coneTypeBerry);

            searchFilters.Children.Add(coneTypeLayout);
        }



        private void FruitColorSearch()
        {
            // Add Type of Plant
            Label fruitColorLabel = new Label { Text = "Fruit Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitColorLabel);

            WrapLayout fruitColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

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
        }

        private void FlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

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
        }

        private void CactusFlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            flowerColorLayout.Children.Add(yellowFlowerColor);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            flowerColorLayout.Children.Add(brownFlowerColor);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            flowerColorLayout.Children.Add(whiteFlowerColor);
       
            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
            flowerColorLayout.Children.Add(orangeFlowerColor);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            flowerColorLayout.Children.Add(pinkFlowerColor);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            flowerColorLayout.Children.Add(purpleFlowerColor);

            searchFilters.Children.Add(flowerColorLayout);
        }

        private void VineFlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            WrapLayout flowerColorLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            flowerColorLayout.Children.Add(yellowFlowerColor);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            flowerColorLayout.Children.Add(blueFlowerColor);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
            flowerColorLayout.Children.Add(greenFlowerColor);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            flowerColorLayout.Children.Add(purpleFlowerColor);

            searchFilters.Children.Add(flowerColorLayout);
        }

       

        private void CactusShapeSearch()
        {
            Label cactusShapeLabel = new Label { Text = "Cactus Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(cactusShapeLabel);

            WrapLayout cactusShapeLayout = new WrapLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };

            SearchCharacteristicIcon flatOneCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Flat1");
            cactusShapeLayout.Children.Add(flatOneCactusShape);

            SearchCharacteristicIcon flatTwoCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Flat2");
            cactusShapeLayout.Children.Add(flatTwoCactusShape);

            SearchCharacteristicIcon roundOneCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Round1");
            cactusShapeLayout.Children.Add(roundOneCactusShape);

            SearchCharacteristicIcon roundTwoCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Round2");
            cactusShapeLayout.Children.Add(roundTwoCactusShape);

            SearchCharacteristicIcon branchedCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Branched");
            cactusShapeLayout.Children.Add(branchedCactusShape);

            SearchCharacteristicIcon flatRoundCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-FlatRound");
            cactusShapeLayout.Children.Add(flatRoundCactusShape);

            searchFilters.Children.Add(cactusShapeLayout);
        }


        private void RunSearch(object sender, EventArgs e)
        {
            App.WoodyPlantRepoLocal.setSearchPlants(plants.ToList());
            InitRunSearch?.Invoke(this, EventArgs.Empty);
        }

        private void CloseSearch(object sender, EventArgs e)
        {
            InitCloseSearch?.Invoke(this, EventArgs.Empty);
        }
    }
}
