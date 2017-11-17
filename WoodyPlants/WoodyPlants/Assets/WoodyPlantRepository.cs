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
        public async Task<ObservableCollection<WoodyPlant>> FilterPlantsBySearchCriteria()
        {
            // get search criteria and plants
            List<WoodySearch> selectCritList = await App.WoodySearchRepo.GetQueryableSearchCriteriaAsync();
            List<WoodyPlant> plants;

            // execute filtering
            if (selectCritList.Count() > 0)
            {
                var predicate = ConstructPredicate(selectCritList);
                //plants = GetAllWoodyPlants();
                plants = conn.Table<WoodyPlant>().AsQueryable().Where(predicate).ToList();
            }
            else
            {
                plants = GetAllWoodyPlants();
            }

            return new ObservableCollection<WoodyPlant>(plants);
                        
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

        // Construct Predicate for plants query, filtering based on selected criteria
        // Solution taken from http://www.albahari.com/nutshell/predicatebuilder.aspx
        private Expression<Func<WoodyPlant, bool>> ConstructPredicate(List<WoodySearch> selectCritList)
        {
            var overallQuery = PredicateBuilder.True<WoodyPlant>();

                
            // Add selected Flower Color characteristics
            var queryFlowerColor = selectCritList.Where(x => x.Characteristic.Contains("FlowerColor"));
            if (queryFlowerColor.Count() > 0)
            {
                var flowerColorQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var flowerColor in queryFlowerColor) { flowerColorQuery = flowerColorQuery.Or(x => x.flowerColor.Contains(flowerColor.SearchString1) || x.flowerColor.Contains(flowerColor.SearchString2) || x.flowerColor.Contains(flowerColor.SearchString3)); }
                overallQuery = overallQuery.And(flowerColorQuery);
            }

            // Add selected Flower Color characteristics
            var queryBarkTexture = selectCritList.Where(x => x.Characteristic.Contains("BarkTexture"));
            if (queryBarkTexture.Count() > 0)
            {
                var barkTextureQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var barkTexture in queryBarkTexture)
                {
                    barkTextureQuery = barkTextureQuery.Or(x => x.barkDescription.Contains(barkTexture.SearchString1) || x.barkDescription.Contains(barkTexture.SearchString2) || x.barkDescription.Contains(barkTexture.SearchString3) || x.barkDescription.Contains(barkTexture.SearchString4) || x.barkDescription.Contains(barkTexture.SearchString5) || x.barkDescription.Contains(barkTexture.SearchString6) || x.barkDescription.Contains(barkTexture.SearchString7) || x.barkDescription.Contains(barkTexture.SearchString8) || x.barkDescription.Contains(barkTexture.SearchString9) || x.barkDescription.Contains(barkTexture.SearchString10));
                }
                overallQuery = overallQuery.And(barkTextureQuery);
            }

            // Add selected Flower Color characteristics
            var queryFlowerCluster = selectCritList.Where(x => x.Characteristic.Contains("FlowerCluster"));
            if (queryFlowerCluster.Count() > 0)
            {
                var flowerClusterQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var flowerCluster in queryFlowerCluster)
                {
                    flowerClusterQuery = flowerClusterQuery.Or(x => x.flowerSize.Contains(flowerCluster.SearchString1) || x.flowerSize.Contains(flowerCluster.SearchString2) || x.flowerSize.Contains(flowerCluster.SearchString3) || x.flowerSize.Contains(flowerCluster.SearchString4) || x.flowerSize.Contains(flowerCluster.SearchString5) || x.flowerSize.Contains(flowerCluster.SearchString6));
                }
                overallQuery = overallQuery.And(flowerClusterQuery);
            }

            // Add selected Flower Color characteristics
            var queryFlowerShape = selectCritList.Where(x => x.Characteristic.Contains("FlowerShape"));
            if (queryFlowerShape.Count() > 0)
            {
                var flowerShapeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var flowerShape in queryFlowerShape)
                {
                    flowerShapeQuery = flowerShapeQuery.Or(x => x.flowerSymmetry.Contains(flowerShape.SearchString1) || x.flowerSymmetry.Contains(flowerShape.SearchString2) || x.flowerSymmetry.Contains(flowerShape.SearchString3) || x.flowerSymmetry.Contains(flowerShape.SearchString4) || x.flowerSymmetry.Contains(flowerShape.SearchString5) || x.flowerSymmetry.Contains(flowerShape.SearchString6));
                }
                overallQuery = overallQuery.And(flowerShapeQuery);
            }

            var queryFruitType = selectCritList.Where(x => x.Characteristic.Contains("FruitType"));
            if (queryFruitType.Count() > 0)
            {
                var fruitTypeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var fruitType in queryFruitType)
                {
                    fruitTypeQuery = fruitTypeQuery.Or(x => x.familyCharacteristics.Contains(fruitType.SearchString1) || x.familyCharacteristics.Contains(fruitType.SearchString2) || x.familyCharacteristics.Contains(fruitType.SearchString3) || x.familyCharacteristics.Contains(fruitType.SearchString4) || x.familyCharacteristics.Contains(fruitType.SearchString5));
                }
                overallQuery = overallQuery.And(fruitTypeQuery);
            }
            
            var queryFruitColor = selectCritList.Where(x => x.Characteristic.Contains("FruitColor"));
            if (queryFruitColor.Count() > 0)
            {
                var fruitColorQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var fruitColor in queryFruitColor)
                {
                    fruitColorQuery = fruitColorQuery.Or(x => x.fruitType.Contains(fruitColor.SearchString1) || x.fruitType.Contains(fruitColor.SearchString2));
                }
                overallQuery = overallQuery.And(fruitColorQuery);
            }
            
            return overallQuery;
        }

    }
}