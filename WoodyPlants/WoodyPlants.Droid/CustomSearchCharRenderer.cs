using System;
using PortableApp.Droid;
using PortableApp;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;

[assembly: ExportRenderer(typeof(SearchCharacteristicIcon), typeof(CustomSearchCharRenderer))]
namespace PortableApp.Droid
{
    class CustomSearchCharRenderer : ButtonRenderer
    {
        /*public CustomSearchCharRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }*/


        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                Control.SetPadding(0, 7, 0, 7);
                //Control.Layout = Conten
                
        }
    }
}