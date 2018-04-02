using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace PortableApp
{

    public class WoodyPlantImageRepositoryLocal
    {
        private List<WoodyPlantImage> allWoodyPlantImages;

        public WoodyPlantImageRepositoryLocal(List<WoodyPlantImage> imagesDB)
        {
            allWoodyPlantImages = imagesDB;
        }

        public void ClearWoodyImagesLocal()
        {
            allWoodyPlantImages = new List<WoodyPlantImage>();
        }

        // return a list of Woody Plant Images saved to the WoodyPlantImage table in the database
        public List<WoodyPlantImage> GetAllWoodyPlantImages()
        {
            return allWoodyPlantImages;
        }

        // return a list of Woody Plant Images for the plant specified
        public List<WoodyPlantImage> PlantImages(int plantId)
        {
            return allWoodyPlantImages.Where(p => p.PlantId.Equals(plantId)).ToList();
        }
        public void ClearWetlandImagesLocal()
        {
            allWoodyPlantImages = new List<WoodyPlantImage>();
        }


    }
}