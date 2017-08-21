using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("woody_plants")]
    public class WoodyPlant
    {
        [PrimaryKey]
        public int plantid { get; set; }

        public string family { get; set; }
        public string scientificnameweber { get; set; }
        public string commonname { get; set; }
        public string othercommonname { get; set; }

        public bool isFavorite { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WoodyPlantImage> Images { get; set; }

        //public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }
        public ImageSource ThumbnailPath { get { return ImageSource.FromResource("WoodyPlants.Resources.Images.image.png"); } }
    }

}
