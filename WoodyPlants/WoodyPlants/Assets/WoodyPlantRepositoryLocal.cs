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

    public class WoodyPlantRepositoryLocal
    {

        public string StatusMessage { get; set; }
        private List<WoodyPlant> allWoodyPlants;
        private List<WoodyPlant> searchPlants;

        public WoodyPlantRepositoryLocal(List<WoodyPlant> allPlantsDB)
        {
            allWoodyPlants = allPlantsDB;

        }

        // return a list of Woodyplants saved to the WoodyPlant table in the database
        public List<WoodyPlant> GetAllWoodyPlants()
        {
            return allWoodyPlants;
        }

        // return a list of Woodyplants saved to the WoodyPlant table in the database
        public async Task<ObservableCollection<WoodyPlant>> GetAllSearchPlants()
        {
            return new ObservableCollection<WoodyPlant>(searchPlants);
        }

        public void setSearchPlants(List<WoodyPlant> searchPlants)
        {
            this.searchPlants = searchPlants;
        }

        public List<string> GetPlantJumpList()
        {
            return allWoodyPlants.Select(x => x.scientificNameWeber.ToString()).Distinct().ToList();
        }

        // return a specific WoodyPlant given an id
        public WoodyPlant GetWoodyPlantByAltId(int Id)
        {
            IEnumerable<WoodyPlant> plants = allWoodyPlants.Where(p => p.plant_id.Equals(Id));

            if (plants != null)
                return plants.First();

            else return null;
        }

        // get plants marked as favorites
        public List<WoodyPlant> GetFavoritePlants()
        {
            return allWoodyPlants.Where(p => p.isFavorite == true).ToList();
        }

        // get plants through term supplied in quick search
        public List<WoodyPlant> WoodyPlantsQuickSearch(string searchTerm)
        {
            return allWoodyPlants.Where(p => p.scientificNameWeber.ToLower().Contains(searchTerm.ToLower()) || p.commonName.ToLower().Contains(searchTerm.ToLower())).ToList();
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
                plants = allWoodyPlants.AsQueryable().Where(predicate).ToList();
            }
            else
            {
                plants = GetAllWoodyPlants();
            }

            return new ObservableCollection<WoodyPlant>(plants);

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