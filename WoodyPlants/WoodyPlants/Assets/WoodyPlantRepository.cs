using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Threading.Tasks;
using System;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensionsAsync.Extensions;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

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

        // return a list of WoodyPlants saved to the WoodyPlant table in the database
        public async Task<ObservableCollection<WoodyPlant>> GetAllWoodyPlantsAsync()
        {
            List<WoodyPlant> list = await connAsync.GetAllWithChildrenAsync<WoodyPlant>(); 
            return new ObservableCollection<WoodyPlant>(list);     
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

        //get all the selected search criteria
        //public async Task<ObservableCollection<WoodyPlant>> FilterPlantsBySearchCriteria()
        //{
        //    // get search criteria and plants
        //    List<WoodySearch> selectCritList = await App.WoodySearchRepo.GetQueryableSearchCriteriaAsync();
        //    ObservableCollection<WoodyPlant> plants = await App.WoodyPlantRepo.GetAllWoodyPlantsAsync();

        //    // execute filtering
        //    Expression<Func<WoodyPlant, bool>> predicate = ConstructPredicate(plants, selectCritList);

        //}

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
        
        public async Task AddOrUpdatePlantAsync(WoodyPlant plant)
        {
            try
            {
                if (string.IsNullOrEmpty(plant.commonName))
                    throw new Exception("Valid plant required");
                await connAsync.InsertOrReplaceWithChildrenAsync(plant);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add {0}. Error: {1}", plant, ex.Message);
            }

        }

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

        //private PredicateBuilder ConstructPredicate(ObservableCollection<WoodyPlant> plants, List<WoodySearch> selectCritList)
        //{
        //    var leafTypeQuery = PredicateBuilder.False<WoodyPlant>();
        //    var queryLeafTypes = selectCritList.Where(x => x.Characteristic.Contains("LeafType"));
        //    if (queryLeafTypes.Count() > 0)
        //    {
        //        foreach (var leafType in queryLeafTypes)
        //        {
        //            leafTypeQuery.Or(x => x.leafType.Contains(leafType.SearchString1));
        //        }
        //    }

        //    plants.Where(leafTypeQuery);
        //}

    }
}