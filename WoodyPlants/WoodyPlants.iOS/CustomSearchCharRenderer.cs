using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PortableApp;
using PortableApp.iOS;
using Foundation;
using UIKit;

[assembly: ExportRenderer(typeof(SearchCharacteristicIcon), typeof(CustomSearchCharRenderer))]
namespace PortableApp.iOS
{
    class CustomSearchCharRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                Control.TitleLabel.LineBreakMode = UIKit.UILineBreakMode.WordWrap;
                Control.HorizontalAlignment = UIKit.UIControlContentHorizontalAlignment.Center;
                Control.ContentMode = UIViewContentMode.Center;

            
        }
    }
}