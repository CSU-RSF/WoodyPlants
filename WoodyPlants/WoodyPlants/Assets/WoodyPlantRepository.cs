using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace PortableApp
{

    public class WoodyPlantRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WoodyPlantRepository()
		{
            //Create the Woody Plant table(only if it's not yet created) 
            //conn.DropTable<WoodyPlant>();
            conn.CreateTable<WoodyPlant>();
            //SeedDB();
        }

        // return a list of WoodyPlants saved to the WoodyPlant table in the database
        public List<WoodyPlant> GetAllWoodyPlants()
        {
            return conn.GetAllWithChildren<WoodyPlant>();
        }

        public void SeedDB()
        {
            // Add seven sample plants
            conn.Insert(new WoodyPlant { plant_id = 1, plant_imported_id = 1, family = "Agave-Agavaceae", scientificNameWeber = "Yucca baccata", commonName = "BANANA YUCCA", scientificNameOther = "datil yucca" });
            conn.Insert(new WoodyPlant { plant_id = 2, plant_imported_id = 2, family = "Agave-Agavaceae", scientificNameWeber = "Yucca glauca", commonName = "YUCCA", scientificNameOther = "spanish bayonet, soapweed" });
            conn.Insert(new WoodyPlant { plant_id = 3, plant_imported_id = 3, family = "Barberry-Berbridaceae", scientificNameWeber = "Berberis fendleri", commonName = "FENDLER BARBERRY", scientificNameOther = "Colorado barberry" });
            conn.Insert(new WoodyPlant { plant_id = 4, plant_imported_id = 4, family = "Barberry-Berbridaceae", scientificNameWeber = "Mahonia repens", commonName = "CREEPING HOLLYGRAPE", scientificNameOther = "Oregon grape, holly-grap, creeping oregon grape" });
            conn.Insert(new WoodyPlant { plant_id = 5, plant_imported_id = 5, family = "Birch-Betulaceae", scientificNameWeber = "Alnus incana", commonName = "THINLEAF ALDER", scientificNameOther = "american speckled alder" });
            conn.Insert(new WoodyPlant { plant_id = 6, plant_imported_id = 6, family = "Birch-Betulaceae", scientificNameWeber = "Betula glandulosa", commonName = "DWARF BIRCH", scientificNameOther = "bog birch" });
            conn.Insert(new WoodyPlant { plant_id = 7, plant_imported_id = 7, family = "Birch-Betulaceae", scientificNameWeber = "Betula occidentalis", commonName = "WESTERN RIVER BIRCH", scientificNameOther = "river birch, rocky mountain birch" });
        }

        public List<string> GetPlantJumpList()
        {
            return GetAllWoodyPlants().Select(x => x.scientificNameWeber.ToString()).Distinct().ToList();
        }

        //// return a specific WoodyPlant given an id
        //public WoodyPlant GetWoodyPlantByAltId(int Id)
        //{
        //    WoodyPlant plant = conn.Table<WoodyPlant>().Where(p => p.id.Equals(Id)).FirstOrDefault();
        //    return conn.GetWithChildren<WoodyPlant>(plant.plantid);
        //}

        // get plants marked as favorites
        public List<WoodyPlant> GetFavoritePlants()
        {
            return GetAllWoodyPlants().Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WoodyPlant> WoodyPlantsQuickSearch(string searchTerm)
        {
            return GetAllWoodyPlants().Where(p => p.scientificNameWeber.ToLower().Contains(searchTerm.ToLower()) || p.commonName.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        //// get current search criteria (saved in db) and return appropriate list of Woody Plants
        //public IEnumerable<WoodyPlant> GetPlantsBySearchCriteria()
        //{
        //    IEnumerable<WoodyPlant> plants = GetAllWoodyPlants();
        //    List<WoodySearch> searchCriteria = new List<WoodySearch>(App.WoodySearchRepo.GetQueryableSearchCriteria());
        //    if (searchCriteria.Count > 0)
        //    {
        //        foreach (WoodySearch criterion in searchCriteria)
        //        {
        //            try
        //            {
        //                if (criterion.Name == "Bentgrass") { plants = plants.Where(x => x.commonname.Contains(criterion.SearchString1)); };
        //                if (criterion.Name == "Poaceae") { plants = plants.Where(x => x.family.Contains(criterion.SearchString1)); };
        //            }
        //            catch
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    return plants;
        //}

        //public async Task AddOrUpdatePlantAsync(WoodyPlant plant)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(plant.commonname))
        //            throw new Exception("Valid plant required");
        //        await connAsync.InsertOrReplaceWithChildrenAsync(plant);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
        //    }

        //}

        public async Task UpdatePlantAsync(WoodyPlant plant)
        {
            try
            {
                await connAsync.UpdateAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to update {0}. Error: {1}", plant, ex.Message);
            }
        }

    }
}