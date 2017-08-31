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

            // Add Family of Plant
            Label leafTypeLabel = new Label { Text = "Leaf Type:", Style = Application.Current.Resources["sectionHeader"] as Style };
            searchFilters.Children.Add(leafTypeLabel);

            leafTypesLayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 5 };
            SearchCharacteristicIcon simpleLeafType = searchCriteria.First(x => x.Characteristic == "LeafType-Simple");
           
            leafTypesLayout.Children.Add(simpleLeafType);
            SearchCharacteristicIcon spineLeafType = searchCriteria.First(x => x.Characteristic == "LeafType-Spine");
           
            leafTypesLayout.Children.Add(spineLeafType);
            
            searchFilters.Children.Add(leafTypesLayout);

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

            searchFilters.Children.Add(flowerColorLayout);

            innerContainer.Children.Add(searchFilters, 0, 0);

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
