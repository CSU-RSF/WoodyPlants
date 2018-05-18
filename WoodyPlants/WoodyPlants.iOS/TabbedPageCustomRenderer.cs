
using PortableApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageCustom))]
namespace PortableApp.iOS
{
   public class TabbedPageCustom : TabbedRenderer
   {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            TabBar.TintColor = UIKit.UIColor.Black;
            TabBar.BarTintColor = UIKit.UIColor.Black;
            TabBar.BackgroundColor = UIKit.UIColor.Black;
        }
    }
}