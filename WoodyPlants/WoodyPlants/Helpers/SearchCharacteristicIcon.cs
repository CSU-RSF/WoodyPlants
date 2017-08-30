using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace PortableApp
{
    public class SearchCharacteristicIcon : ImageButton
    {
        public static readonly BindableProperty CharacteristicProperty = BindableProperty.Create("Characteristic", typeof(string), typeof(ImageButton), null);
        public static readonly BindableProperty QueryProperty = BindableProperty.Create("Query", typeof(bool), typeof(ImageButton), false);
        public static readonly BindableProperty Column1Property = BindableProperty.Create("Column1", typeof(string), typeof(ImageButton), null);
        public static readonly BindableProperty SearchString1Property = BindableProperty.Create("SearchString1", typeof(string), typeof(ImageButton), null);

        public string Characteristic
        {
            get { return (string)GetValue(CharacteristicProperty); }
            set { SetValue(CharacteristicProperty, value); }
        }

        public bool Query
        {
            get { return (bool)GetValue(QueryProperty); }
            set { SetValue(QueryProperty, value); }
        }

        public string Column1
        {
            get { return GetValue(Column1Property) as string; }
            set { SetValue(Column1Property, value); }
        }

        public string SearchString1
        {
            get { return GetValue(SearchString1Property) as string; }
            set { SetValue(SearchString1Property, value); }
        }

        public SearchCharacteristicIcon()
        {
            TextColor = Color.White;
            BorderColor = Color.White;
            BackgroundColor = Color.Transparent;
        }

    }
}
