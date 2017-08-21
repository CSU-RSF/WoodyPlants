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
            conn.DropTable<WoodyPlant>();
            conn.CreateTable<WoodyPlant>();
            SeedDB();
        }

        // return a list of WoodyPlants saved to the WoodyPlant table in the database
        public List<WoodyPlant> GetAllWoodyPlants()
        {
            return conn.GetAllWithChildren<WoodyPlant>();
        }

        public void SeedDB()
        {
            conn.Insert(new WoodyPlant { plantid = 1, family = "Agave-Agavaceae", scientificnameweber = "Yucca baccata", commonname = "BANANA YUCCA", othercommonname = "datil yucca" });
            conn.Insert(new WoodyPlant { plantid = 2, family = "Agave-Agavaceae", scientificnameweber = "Yucca glauca", commonname = "YUCCA", othercommonname = "spanish bayonet, soapweed" });
            conn.Insert(new WoodyPlant { plantid = 3, family = "Barberry-Berbridaceae", scientificnameweber = "Berberis fendleri", commonname = "FENDLER BARBERRY", othercommonname = "Colorado barberry" });
            conn.Insert(new WoodyPlant { plantid = 4, family = "Barberry-Berbridaceae", scientificnameweber = "Mahonia repens", commonname = "CREEPING HOLLYGRAPE", othercommonname = "Oregon grape, holly-grap, creeping oregon grape" });
            conn.Insert(new WoodyPlant { plantid = 5, family = "Birch-Betulaceae", scientificnameweber = "Alnus incana", commonname = "THINLEAF ALDER", othercommonname = "american speckled alder" });
            conn.Insert(new WoodyPlant { plantid = 6, family = "Birch-Betulaceae", scientificnameweber = "Betula glandulosa", commonname = "DWARF BIRCH", othercommonname = "bog birch" });
            conn.Insert(new WoodyPlant { plantid = 7, family = "Birch-Betulaceae", scientificnameweber = "Betula occidentalis", commonname = "WESTERN RIVER BIRCH", othercommonname = "river birch, rocky mountain birch" });
        }

        public List<string> GetPlantJumpList()
        {
            return GetAllWoodyPlants().Select(x => x.scientificnameweber.ToString()).Distinct().ToList();
        }

        //// return a specific WoodyPlant given an id
        //public WoodyPlant GetWoodyPlantByAltId(int Id)
        //{
        //    WoodyPlant plant = conn.Table<WoodyPlant>().Where(p => p.id.Equals(Id)).FirstOrDefault();
        //    return conn.GetWithChildren<WoodyPlant>(plant.plantid);
        //}

        //// get plants marked as favorites
        //public List<WoodyPlant> GetFavoritePlants()
        //{
        //    return GetAllWoodyPlants().Where(p => p.isFavorite == true).ToList();
        //}

        // get plants through term supplied in quick search
        public List<WoodyPlant> WoodyPlantsQuickSearch(string searchTerm)
        {
            return GetAllWoodyPlants().Where(p => p.scientificnameweber.ToLower().Contains(searchTerm.ToLower())).ToList();
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