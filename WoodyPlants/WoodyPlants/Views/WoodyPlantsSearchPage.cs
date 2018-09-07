using PortableApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
        Button searchButton;
        StackLayout searchFilters;

        Grid innerContainer;
        ScrollView scrollView;

        SearchCharacteristicIcon deciduousPlantType;
        SearchCharacteristicIcon coniferPlantType;
        SearchCharacteristicIcon vinePlantType;
        SearchCharacteristicIcon cactiPlantType;

        protected async override void OnAppearing()
        {
            IsLoading = true;

            // ResetSearchFilters();
            //changed this to local
            if (plants == null)
            {
                if (App.WoodyPlantRepoLocal.GetAllWoodyPlants().Count > 0)
                {
                    plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());
                    base.OnAppearing();
                }
                else
                {
                    plants = new ObservableCollection<WoodyPlant>(await externalConnection.GetAllPlants());
                    App.WoodyPlantRepoLocal = new WoodyPlantRepositoryLocal(new List<WoodyPlant>(plants));

                    base.OnAppearing();
                }
            }

            searchButton.Text = "VIEW " + plants.Count() + " RESULTS";

            IsLoading = false;

            innerContainer.Children.Add(scrollView, 0, 0);
        }
        public WoodyPlantsSearchPage()
        {
            //plants = new ObservableCollection<WoodyPlant>(App.WoodyPlantRepoLocal.GetAllWoodyPlants());
            searchCriteriaDB = new ObservableCollection<WoodySearch>(App.WoodySearchRepo.GetAllWoodySearchCriteria());
            searchCriteria = SearchCharacteristicIconsCollection();

            // Turn off navigation bar and initialize pageContainer
            NavigationPage.SetHasNavigationBar(this, false);
            AbsoluteLayout pageContainer = ConstructPageContainer();

            // Initialize grid for inner container
            innerContainer = new Grid {
                //Padding = new Thickness(20, Device.OnPlatform(30, 20, 20), 20, 20),
                BackgroundColor = Color.FromHex("00000000"),
                RowSpacing = 0
            };
            innerContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            searchFilters = new StackLayout { Spacing = 10 };

            // Add Type of Plant
            Label plantTypeLabel = new Label { Text = "Plant Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(plantTypeLabel);

            var plantTypeLayout = new Grid();
            plantTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            plantTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            plantTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            plantTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            deciduousPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Deciduous");
            //deciduousPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(deciduousPlantType,0,0);

            coniferPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Conifer");
            //coniferPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(coniferPlantType, 1, 0);

            vinePlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Vine");
            //vinePlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(vinePlantType, 0, 1);

            cactiPlantType = searchCriteria.First(x => x.Characteristic == "PlantType-Cacti");
            //cactiPlantType.Clicked += ChangeSearchCharacteristics;
            plantTypeLayout.Children.Add(cactiPlantType, 1, 1);

            searchFilters.Children.Add(plantTypeLayout);




            scrollView = new ScrollView()
            {
                Content = searchFilters,
                Orientation = ScrollOrientation.Vertical,
                BackgroundColor = Color.FromHex("00000000"),     
                         
            };
            

            // Add Search/Reset button group
            Grid searchButtons = new Grid { BackgroundColor = Color.FromHex("00000000"), RowSpacing = 10/*Margin = new Thickness(0,5,0,5)*/ };
            //searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            searchButtons.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });

            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchButtons.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        
            Button resetButton = new Button { Text = "RESET", Style = Application.Current.Resources["semiTransparentWhiteButton"] as Style };
            resetButton.Clicked += ResetSearchFilters;
            searchButtons.Children.Add(resetButton, 0, 0);

            searchButton = new Button { Style = Application.Current.Resources["semiTransparentWhiteButton2"] as Style };
            //searchButton.Text = "VIEW " + plants.Count() + " RESULTS";
            searchButton.Clicked += RunSearch;
            searchButtons.Children.Add(searchButton, 1, 0);

            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });

            BoxView box1 = new BoxView
            {
                Color = Color.FromHex("88000000"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand               
            };

            BoxView box2 = new BoxView
            {
                Color = Color.FromHex("88000000"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            innerContainer.Children.Add(box1,0,1);

            innerContainer.Children.Add(searchButtons, 0, 1);

            // Add Close button
            Button closeButton = new Button
            {
                Style = Application.Current.Resources["outlineButton"] as Style,
                Text = "CLOSE",
                BorderRadius = Device.OnPlatform(1, 1, 0),
                //Padding = new Thickness(0,10,0,10)
            };
            closeButton.Clicked += CloseSearch;
            innerContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(45) });

            innerContainer.Children.Add(box2, 0, 2);

            innerContainer.Children.Add(closeButton, 0, 2);

            // Add inner container to page container and set as page content
            pageContainer.Children.Add(innerContainer, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

            Content = pageContainer;


        }

        private async void ResetSearchFilters(object sender, EventArgs e)
        {
           

            foreach (var searchCrit in searchCriteria)
            {
                //searchCrit.BorderWidth = 0;
                searchCrit.BorderColor = Color.White;
                searchCrit.BackgroundColor = Color.White;
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
        private async void ResetSearchFilters()
        {
            

            foreach (var searchCrit in searchCriteria)
            {
                //searchCrit.BorderWidth = 0;
                searchCrit.BorderColor = Color.White;
                searchCrit.BackgroundColor = Color.White;
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

            /*deciduousPlantType.BorderWidth = 0;
            coniferPlantType.BorderWidth = 0;
            vinePlantType.BorderWidth = 0;
            cactiPlantType.BorderWidth = 0;*/
            deciduousPlantType.BorderColor = Color.White;
            coniferPlantType.BorderColor = Color.White;
            vinePlantType.BorderColor = Color.White;
            cactiPlantType.BorderColor = Color.White;

            deciduousPlantType.BackgroundColor = Color.White;
            coniferPlantType.BackgroundColor = Color.White;
            vinePlantType.BackgroundColor = Color.White;
            cactiPlantType.BackgroundColor = Color.White;

            correspondingDBRecord1.Query = deciduousPlantType.Query = false;
            correspondingDBRecord2.Query = coniferPlantType.Query = false;
            correspondingDBRecord3.Query = vinePlantType.Query = false;
            correspondingDBRecord4.Query = cactiPlantType.Query = false;

            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord1);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord2);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord3);
            await App.WoodySearchRepo.UpdateSearchCriteriaAsync(correspondingDBRecord4);
        }


        private void ChangeSearchCharacteristics(object sender, EventArgs e)
        {
            innerContainer.Children.Remove(scrollView);

            IsLoading = true;

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

            innerContainer.Children.Add(scrollView, 0, 0);

            IsLoading = false;

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
                    //button.BorderWidth = 0;
                    button.BorderColor = Color.White;
                    button.BackgroundColor = Color.White;
                }
                else if (button.Query == false)
                {
                    correspondingDBRecord.Query = button.Query = true;
                    //  button.BorderWidth = 1;
                    button.BorderColor = Color.LightGreen;
                    button.BackgroundColor = Color.LightGreen;
                }
            }
            else
            {
                if (button.Query == true)
                {
                    ResetSearchFilters(sender,e);
                    //????????
                    button.BorderColor = Color.White;
                    button.BackgroundColor = Color.White;
                    correspondingDBRecord.Query = button.Query = false;
                }
                else if (button.Query == false)
                {
                    ResetTypeButtons(sender, e);
                    //ResetSearchFilters(sender, e);
                   // button.BorderWidth = 1;
                    button.BorderColor = Color.LightGreen;
                    button.BackgroundColor = Color.LightGreen;
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
                //item.BorderWidth = item.Query ? 1 : 0;
                item.BorderWidth = 2;
                searchCriteria.Add(item);
            }
            return searchCriteria;
        }

        private void LeafShapeSearch()
        {
            Label leafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafShapeLabel);

            var leafShapeLayout = new Grid();
            leafShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            leafShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            SearchCharacteristicIcon narrowLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Narrow");
            leafShapeLayout.Children.Add(narrowLeafShape, 0,0);

            SearchCharacteristicIcon deltoidLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Deltoid");
            leafShapeLayout.Children.Add(deltoidLeafShape,1,0);

            SearchCharacteristicIcon orbicularLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Orbicular");
            leafShapeLayout.Children.Add(orbicularLeafShape,0,1);

            SearchCharacteristicIcon oblanceolateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Oblanceolate");
            leafShapeLayout.Children.Add(oblanceolateLeafShape,1,1);

            SearchCharacteristicIcon palmatelyLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Palmately");
            leafShapeLayout.Children.Add(palmatelyLeafShape,0,2);

            SearchCharacteristicIcon pinnateLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Pinnate");
            leafShapeLayout.Children.Add(pinnateLeafShape,1,2);

            SearchCharacteristicIcon lobedLeafShape = searchCriteria.First(x => x.Characteristic == "LeafShape-Lobed");
            leafShapeLayout.Children.Add(lobedLeafShape, 0, 3);

            searchFilters.Children.Add(leafShapeLayout);
        }
        private void VineLeafShapeSearch()
        {
            Label vineLeafShapeLabel = new Label { Text = "Leaf Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(vineLeafShapeLabel);

            var vineLeafShapeLayout = new Grid();
            vineLeafShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            vineLeafShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            vineLeafShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon deltoidLeafShape = searchCriteria.First(x => x.Characteristic == "ShapeVineLeaf-Shape1");
            vineLeafShapeLayout.Children.Add(deltoidLeafShape,0,0);

            SearchCharacteristicIcon oblanceolateLeafShape = searchCriteria.First(x => x.Characteristic == "ShapeVineLeaf-Shape2");
            vineLeafShapeLayout.Children.Add(oblanceolateLeafShape,1,0);

            searchFilters.Children.Add(vineLeafShapeLayout);
        }
        private void NeedleShapeSearch()
        {
            Label needleShapeLabel = new Label { Text = "Needle Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(needleShapeLabel);

            var needleLayout = new Grid();
            needleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            needleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            needleLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            needleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            needleLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
           

            SearchCharacteristicIcon twoClusterShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-TwoCluster");
            needleLayout.Children.Add(twoClusterShape, 0, 0);

            SearchCharacteristicIcon fiveClusterShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-FiveCluster");
            needleLayout.Children.Add(fiveClusterShape, 1, 0);

            SearchCharacteristicIcon flatShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Flat");
            needleLayout.Children.Add(flatShape, 0, 1);

            SearchCharacteristicIcon sharpShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Sharp");
            needleLayout.Children.Add(sharpShape, 1, 1);

            SearchCharacteristicIcon scaleShape = searchCriteria.First(x => x.Characteristic == "NeedleShape-Scale");
            needleLayout.Children.Add(scaleShape, 0, 2);

            searchFilters.Children.Add(needleLayout);
        }
        private void LeafArrangementSearch()
        {
            // Add Type of Plant
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };

            searchFilters.Children.Add(leafArrangementLabel);

            var leafArrangementLayout = new Grid();
            leafArrangementLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            

            SearchCharacteristicIcon alternateLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Alternate");
            leafArrangementLayout.Children.Add(alternateLeafArrangement,0,0);

            SearchCharacteristicIcon oppositeLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Opposite");
            leafArrangementLayout.Children.Add(oppositeLeafArrangement, 1, 0);

            SearchCharacteristicIcon whorledLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Whorled");
            leafArrangementLayout.Children.Add(whorledLeafArrangement, 0, 1);

            SearchCharacteristicIcon basalLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Basal");
            leafArrangementLayout.Children.Add(basalLeafArrangement, 1, 1);

            searchFilters.Children.Add(leafArrangementLayout);
        }
        private void VineLeafArrangementSearch()
        {
            // Add Type of Plant
            Label leafArrangementLabel = new Label { Text = "Leaf Arrangement:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafArrangementLabel);

            var leafArrangementLayout = new Grid();
            leafArrangementLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            leafArrangementLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon alternateLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Alternate");
            leafArrangementLayout.Children.Add(alternateLeafArrangement, 0, 0);

            SearchCharacteristicIcon oppositeLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Opposite");
            leafArrangementLayout.Children.Add(oppositeLeafArrangement, 1, 0);

            SearchCharacteristicIcon whorledLeafArrangement = searchCriteria.First(x => x.Characteristic == "LeafArrangement-Whorled");
            leafArrangementLayout.Children.Add(whorledLeafArrangement, 0, 1);

            searchFilters.Children.Add(leafArrangementLayout);
        }
        private void TwigTextureSearch()
        {
            // Add Type of Plant
            Label twigTextureLabel = new Label { Text = "Twig Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(twigTextureLabel);

            var twigTextureLayout = new Grid();
            twigTextureLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            twigTextureLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            twigTextureLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            twigTextureLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            twigTextureLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            SearchCharacteristicIcon hairyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Hairy");
            twigTextureLayout.Children.Add(hairyTwigTexture, 0, 0);

            SearchCharacteristicIcon smoothTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Smooth");
            twigTextureLayout.Children.Add(smoothTwigTexture, 1, 0);

            SearchCharacteristicIcon roughTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Rough");
            twigTextureLayout.Children.Add(roughTwigTexture, 0, 1);

            SearchCharacteristicIcon peelingTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Peeling");
            twigTextureLayout.Children.Add(peelingTwigTexture, 1, 1);

            SearchCharacteristicIcon thornyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Thorny");
            twigTextureLayout.Children.Add(thornyTwigTexture, 0, 2);

            SearchCharacteristicIcon stickyTwigTexture = searchCriteria.First(x => x.Characteristic == "TwigTexture-Sticky");
            twigTextureLayout.Children.Add(stickyTwigTexture, 1, 2);

            searchFilters.Children.Add(twigTextureLayout);
        }
        private void BarkTextureSearch()
        {
            // Add Type of Plant
            Label barkTextureLabel = new Label { Text = "Bark Texture:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(barkTextureLabel);

            var barkTextureLayout = new Grid();
            barkTextureLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            barkTextureLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            barkTextureLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            barkTextureLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            

            SearchCharacteristicIcon barkTextureSmooth = searchCriteria.First(x => x.Characteristic == "BarkTexture-Smooth");
            barkTextureLayout.Children.Add(barkTextureSmooth, 0, 0);

            SearchCharacteristicIcon barkTextureBumpy = searchCriteria.First(x => x.Characteristic == "BarkTexture-Bumpy");
            barkTextureLayout.Children.Add(barkTextureBumpy, 1, 0);

            SearchCharacteristicIcon barkTexturePeeling = searchCriteria.First(x => x.Characteristic == "BarkTexture-Peeling");
            barkTextureLayout.Children.Add(barkTexturePeeling, 0, 1);

            SearchCharacteristicIcon barkTextureFurrowed = searchCriteria.First(x => x.Characteristic == "BarkTexture-Furrowed");
            barkTextureLayout.Children.Add(barkTextureFurrowed, 1, 1);

            searchFilters.Children.Add(barkTextureLayout);
        }
        private void FlowerCluserSearch()
        {
            // Add Type of Plant
            Label flowerClusterLabel = new Label { Text = "Flower Cluster:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerClusterLabel);

            var flowerClusterLayout = new Grid();
            flowerClusterLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerClusterLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerClusterLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerClusterLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
           

            SearchCharacteristicIcon flowerClusterDense = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Dense");
            flowerClusterLayout.Children.Add(flowerClusterDense, 0, 0);

            SearchCharacteristicIcon flowerClusterLoose = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Loose");
            flowerClusterLayout.Children.Add(flowerClusterLoose, 1, 0);

            SearchCharacteristicIcon flowerClusterSolitary = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Solitary");
            flowerClusterLayout.Children.Add(flowerClusterSolitary, 0, 1);

            SearchCharacteristicIcon flowerClusterCatkin = searchCriteria.First(x => x.Characteristic == "FlowerCluster-Catkin");
            flowerClusterLayout.Children.Add(flowerClusterCatkin, 1, 1);

            searchFilters.Children.Add(flowerClusterLayout);
        }
        private void FlowerShapeSearch()
        {
            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);

            var flowerShapeLayout = new Grid();
            flowerShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });



            SearchCharacteristicIcon flowerShapeInconspicuous = searchCriteria.First(x => x.Characteristic == "FlowerShape-Inconspicuous");
            flowerShapeLayout.Children.Add(flowerShapeInconspicuous, 0 ,0);

            SearchCharacteristicIcon flowerShapeRound = searchCriteria.First(x => x.Characteristic == "FlowerShape-Round");
            flowerShapeLayout.Children.Add(flowerShapeRound, 1, 0);

            SearchCharacteristicIcon flowerShapeBellShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-BellShaped");
            flowerShapeLayout.Children.Add(flowerShapeBellShaped, 0, 1);

            SearchCharacteristicIcon flowerShapeCupShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-CupShaped");
            flowerShapeLayout.Children.Add(flowerShapeCupShaped, 1, 1);

            SearchCharacteristicIcon flowerShapeStarShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-StarShaped");
            flowerShapeLayout.Children.Add(flowerShapeStarShaped, 0, 2);

            SearchCharacteristicIcon flowerShapeOther = searchCriteria.First(x => x.Characteristic == "FlowerShape-Other");
            flowerShapeLayout.Children.Add(flowerShapeOther, 1, 2);

            searchFilters.Children.Add(flowerShapeLayout);
        }
        private void VineFlowerShapeSearch()
        {
            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label flowerShapeLabel = new Label { Text = "Flower Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerShapeLabel);


            var flowerShapeLayout = new Grid();
            flowerShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon flowerShapeStarShaped = searchCriteria.First(x => x.Characteristic == "FlowerShape-StarShaped");
            flowerShapeLayout.Children.Add(flowerShapeStarShaped, 0, 0);

            SearchCharacteristicIcon flowerShapeRound = searchCriteria.First(x => x.Characteristic == "FlowerShape-Round");
            flowerShapeLayout.Children.Add(flowerShapeRound, 1, 0);

            searchFilters.Children.Add(flowerShapeLayout);
        }
        private void FruitTypeSearch()
        {
            
                // Add Type of PlantFIXXXXXXXXXXXXXX
                Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
                searchFilters.Children.Add(fruitTypeLabel);
              
                var fruitTypeLayout = new Grid();
                fruitTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                fruitTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                fruitTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                fruitTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                fruitTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                SearchCharacteristicIcon fruitTypeDrySeed = searchCriteria.First(x => x.Characteristic == "FruitType-DrySeed");
                fruitTypeLayout.Children.Add(fruitTypeDrySeed,0,0);

                SearchCharacteristicIcon fruitTypeAcorn = searchCriteria.First(x => x.Characteristic == "FruitType-Acorn");
                fruitTypeLayout.Children.Add(fruitTypeAcorn, 1, 0);

                SearchCharacteristicIcon fruitTypeFleshy = searchCriteria.First(x => x.Characteristic == "FruitType-Fleshy");
                fruitTypeLayout.Children.Add(fruitTypeFleshy, 0, 1);

                SearchCharacteristicIcon fruitTypeCone = searchCriteria.First(x => x.Characteristic == "FruitType-Cone");
                fruitTypeLayout.Children.Add(fruitTypeCone, 1, 1);

                SearchCharacteristicIcon fruitTypeCapsule = searchCriteria.First(x => x.Characteristic == "FruitType-Capsule");
                fruitTypeLayout.Children.Add(fruitTypeCapsule, 0, 2);

                SearchCharacteristicIcon fruitTypeSamara = searchCriteria.First(x => x.Characteristic == "FruitType-Samara");
                fruitTypeLayout.Children.Add(fruitTypeSamara, 1, 2);

                searchFilters.Children.Add(fruitTypeLayout);

        }
        private void VineFruitTypeSearch()
        {

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label fruitTypeLabel = new Label { Text = "Fruit Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(fruitTypeLabel);

            var fruitTypeLayout = new Grid();
            fruitTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            fruitTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            fruitTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon fruitTypeFleshy = searchCriteria.First(x => x.Characteristic == "FruitType-Fleshy");
            fruitTypeLayout.Children.Add(fruitTypeFleshy, 0, 0);
        
            SearchCharacteristicIcon fruitTypeCapsule = searchCriteria.First(x => x.Characteristic == "FruitType-DrySeed");
            fruitTypeLayout.Children.Add(fruitTypeCapsule,1,0);
     
            searchFilters.Children.Add(fruitTypeLayout);

        }
        private void ConeTypeSearch()
        {

            // Add Type of PlantFIXXXXXXXXXXXXXX
            Label coneTypeLabel = new Label { Text = "Cone Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(coneTypeLabel);

            var coneTypeLayout = new Grid();
            coneTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            coneTypeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            coneTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            coneTypeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon coneTypeCone = searchCriteria.First(x => x.Characteristic == "ConeType-SolidCone");
            coneTypeLayout.Children.Add(coneTypeCone,0,0);

            SearchCharacteristicIcon conePapTypeCone = searchCriteria.First(x => x.Characteristic == "ConeType-PaperyCone");
            coneTypeLayout.Children.Add(conePapTypeCone,1,0);

            SearchCharacteristicIcon coneTypeBerry = searchCriteria.First(x => x.Characteristic == "ConeType-Berry");
            coneTypeLayout.Children.Add(coneTypeBerry,0,1);

            searchFilters.Children.Add(coneTypeLayout);
        }
        private void FlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            var flowerColorLayout = new Grid();
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            //yellowFlowerColor.BackgroundColor = Color.Yellow;
           // yellowFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(yellowFlowerColor, 0, 0);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            //blueFlowerColor.BackgroundColor = Color.Blue;
           // blueFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(blueFlowerColor, 1, 0);

            SearchCharacteristicIcon redFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Red");
            //redFlowerColor.BackgroundColor = Color.Red;
            //redFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(redFlowerColor, 0, 1);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            //brownFlowerColor.BackgroundColor = Color.SaddleBrown;
            //brownFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(brownFlowerColor, 1, 1);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            //whiteFlowerColor.BackgroundColor = Color.White;
            //whiteFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(whiteFlowerColor, 0, 2);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
            //greenFlowerColor.BackgroundColor = Color.Green;
            //greenFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(greenFlowerColor, 1, 2);

            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
            //orangeFlowerColor.BackgroundColor = Color.Orange;
            //orangeFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(orangeFlowerColor, 0, 3);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            //pinkFlowerColor.BackgroundColor = Color.Pink;
            //pinkFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(pinkFlowerColor, 1, 3);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            //purpleFlowerColor.BackgroundColor = Color.Purple;
            //purpleFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(purpleFlowerColor, 0, 4);

            searchFilters.Children.Add(flowerColorLayout);
        }
        private void CactusFlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            var flowerColorLayout = new Grid();
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
            //yellowFlowerColor.BackgroundColor = Color.Yellow;
           // yellowFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(yellowFlowerColor, 0, 0);

            SearchCharacteristicIcon brownFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Brown");
            //brownFlowerColor.BackgroundColor = Color.SaddleBrown;
            //brownFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(brownFlowerColor, 1, 0);

            SearchCharacteristicIcon whiteFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-White");
            //whiteFlowerColor.BackgroundColor = Color.White;
            //whiteFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(whiteFlowerColor, 0, 1);
       
            SearchCharacteristicIcon orangeFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Orange");
           // orangeFlowerColor.BackgroundColor = Color.Orange;
            //orangeFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(orangeFlowerColor, 1, 1);

            SearchCharacteristicIcon pinkFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Pink");
            //pinkFlowerColor.BackgroundColor = Color.Pink;
            //pinkFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(pinkFlowerColor, 0, 2);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            //purpleFlowerColor.BackgroundColor = Color.Purple;
            //purpleFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(purpleFlowerColor, 1, 2);

            searchFilters.Children.Add(flowerColorLayout);
        }
        private void VineFlowerColorSearch()
        {
            // Add Type of Plant
            Label flowerColorLabel = new Label { Text = "Flower Color:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(flowerColorLabel);

            var flowerColorLayout = new Grid();
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            flowerColorLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            SearchCharacteristicIcon yellowFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Yellow");
           // yellowFlowerColor.BackgroundColor = Color.Yellow;
            //yellowFlowerColor.TextColor = Color.Black;
            flowerColorLayout.Children.Add(yellowFlowerColor, 0, 0);

            SearchCharacteristicIcon blueFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Blue");
            //blueFlowerColor.BackgroundColor = Color.Blue;
            //blueFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(blueFlowerColor, 1, 0);

            SearchCharacteristicIcon greenFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Green");
           // greenFlowerColor.BackgroundColor = Color.Green;
            //greenFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(greenFlowerColor, 0, 1);

            SearchCharacteristicIcon purpleFlowerColor = searchCriteria.First(x => x.Characteristic == "FlowerColor-Purple");
            //purpleFlowerColor.BackgroundColor = Color.Purple;
            //purpleFlowerColor.TextColor = Color.White;
            flowerColorLayout.Children.Add(purpleFlowerColor, 1, 1);

            searchFilters.Children.Add(flowerColorLayout);
        }
        private void CactusShapeSearch()
        {
            Label cactusShapeLabel = new Label { Text = "Cactus Shape:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(cactusShapeLabel);

            var cactusShapeLayout = new Grid();
            cactusShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            cactusShapeLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            cactusShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cactusShapeLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            SearchCharacteristicIcon flatOneCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Flat1");
            cactusShapeLayout.Children.Add(flatOneCactusShape, 0, 0);

            SearchCharacteristicIcon roundOneCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Sphere");
            cactusShapeLayout.Children.Add(roundOneCactusShape, 1, 0);

            SearchCharacteristicIcon branchedCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Branched");
            cactusShapeLayout.Children.Add(branchedCactusShape, 0, 1);

            SearchCharacteristicIcon flatRoundCactusShape = searchCriteria.First(x => x.Characteristic == "CactusShape-Cylinder");
            cactusShapeLayout.Children.Add(flatRoundCactusShape, 1, 1);

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
