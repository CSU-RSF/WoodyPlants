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
    [Table("woody_plant_images")]
    public class WoodyPlantImage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(WoodyPlant))]
        public int PlantId { get; set; }

        [MaxLength(250), Unique]
        public string FileName { get; set; }

        [MaxLength(250)]
        public string Credit { get; set; }
        public string ImageCredit { get { return string.Format("Image Credit: {0}", Credit); } }

        //public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }
        public ImageSource ImagePath { get { return ImageSource.FromResource("WoodyPlants.Resources.Images.image.png"); } }
    }

}
