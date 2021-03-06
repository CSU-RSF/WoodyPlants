﻿using PortableApp.Models;
using PortableApp.Views;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PortableApp.Views
{
    public partial class WoodyPlantDetailPage : TabbedPage
    {
        public WoodySetting selectedTabSetting = App.WoodySettingsRepo.GetSetting("SelectedTab");

        public WoodyPlantDetailPage(WoodyPlant plant, bool streaming, ObservableCollection<WoodyPlant> plants = null)
        {
            GC.Collect();
            NavigationPage.SetHasNavigationBar(this, false);
            var helpers = new ViewHelpers();

            Children.Add(new WoodyPlantImagesPage(plant, plants, streaming) { Title = "IMAGES", Icon = "images.png" });
            Children.Add(new WoodyPlantInfoPage(plant, plants) { Title = "INFO", Icon = "info.png" });
            Children.Add(new WoodyPlantEcologyPage(plant, plants) { Title = "ECOLOGY", Icon = "ecology.png" });
            Children.Add(new WoodyPlantRangePage(plant, plants, streaming) { Title = "RANGE", Icon = "range.png" });
            BarBackgroundColor = Color.Black;
            BarTextColor = Color.White;
            BackgroundColor = Color.Black;
            if (selectedTabSetting != null)
                SelectedItem = Children[Convert.ToInt32(selectedTabSetting.valueint)];
            else
                SelectedItem = Children[0];

            this.CurrentPageChanged += RememberPageChange;
        }

        private async void RememberPageChange(object sender, EventArgs e)
        {
            int index = this.Children.IndexOf(this.CurrentPage);
            await App.WoodySettingsRepo.AddOrUpdateSettingAsync(new WoodySetting { name = "SelectedTab", valueint = (long?)index } );
        }
    }
}
