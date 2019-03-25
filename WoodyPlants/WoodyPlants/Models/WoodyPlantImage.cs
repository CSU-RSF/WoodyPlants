using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;


namespace PortableApp.Models
{
 
    public class WoodyPlantImage
    {
        public string imageName { get; set; }
        public string ImagePathStreamed { get; set; }
        public string ImagePathDownloaded { get; set; }

        public WoodyPlantImage(string imageName, IFolder rootFolder)
        {
            this.imageName = imageName;

            ImagePathDownloaded = rootFolder.Path + "/Images/" + imageName + ".jpg";

            ImagePathStreamed = "http://sdt1.agsci.colostate.edu/mobileapi/api/woody/image_name/" + imageName;
        }

    }
}
