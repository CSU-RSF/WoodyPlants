using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace PortableApp
{

    public class WoodyPlantImageRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WoodyPlantImageRepository()
		{
            //Create the Woody Plant table(only if it's not yet created) 
            conn.DropTable<WoodyPlantImage>();
            conn.CreateTable<WoodyPlantImage>();
            SeedDB();
        }

        // return a list of Woody Plant Images saved to the WoodyPlantImage table in the database
        public List<WoodyPlantImage> GetAllWetlandPlantImages()
        {
            return (from p in conn.Table<WoodyPlantImage>() select p).ToList();
        }

        // return a list of Woody Plant Images for the plant specified
        public List<WoodyPlantImage> PlantImages(int plantId)
        {
            return conn.Table<WoodyPlantImage>().Where(p => p.PlantId.Equals(plantId)).ToList();
        }

        public void SeedDB()
        { 
            // Add three placeholder images for each plant
            for (int i = 1; i <= 7; i++)
            {
                conn.Insert(new WoodyPlantImage { PlantId = i, Credit = "person1" });
                conn.Insert(new WoodyPlantImage { PlantId = i, Credit = "person2" });
                conn.Insert(new WoodyPlantImage { PlantId = i, Credit = "person3" });
            }
        }
    }
}