using System.Collections.Generic;
using System.Linq;
using WoodyPlants.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;

namespace WoodyPlants
{

    public class WoodyPlantRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WoodyPlantRepository()
		{
            // Create the Wetland Plant table (only if it's not yet created) 
            //conn.DropTable<WoodyPlant>();
            conn.CreateTable<WoodyPlant>();
            ///SeedDB();
		}

        // return a list of WoodyPlants saved to the WoodyPlant table in the database
        public List<WoodyPlant> GetAllWoodyPlants()
        {
            return conn.GetAllWithChildren<WoodyPlant>();
        }

        //public void SeedDB()
        //{
        //    conn.Insert(new WoodyPlant { plantid = 1, plantname = "Test" });
        //}

        //public List<string> GetPlantJumpList()
        //{
        //    return GetAllWoodyPlants().Select(x => x.scinameauthorstripped[0].ToString()).Distinct().ToList();
        //}

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

        //// get plants through term supplied in quick search
        //public List<WoodyPlant> WoodyPlantsQuickSearch(string searchTerm)
        //{
        //    return GetAllWoodyPlants().Where(p => p.scinameauthorstripped.ToLower().Contains(searchTerm.ToLower()) || p.commonname.ToLower().Contains(searchTerm.ToLower())).ToList();
        //}

        //// get current search criteria (saved in db) and return appropriate list of Wetland Plants
        //public IEnumerable<WoodyPlant> GetPlantsBySearchCriteria()
        //{
        //    IEnumerable<WoodyPlant> plants = GetAllWoodyPlants();
        //    List<WetlandSearch> searchCriteria = new List<WetlandSearch>(App.WetlandSearchRepo.GetQueryableSearchCriteria());
        //    if (searchCriteria.Count > 0)
        //    {
        //        foreach (WetlandSearch criterion in searchCriteria)
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

        //public async Task UpdatePlantAsync(WoodyPlant plant)
        //{
        //    try
        //    {
        //        await connAsync.UpdateAsync(plant);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to update {0}. Error: {1}", plant, ex.Message);
        //    }
        //}

    }
}