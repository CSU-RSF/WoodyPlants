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

            var queryPlantType = selectCritList.Where(x => x.Characteristic.Contains("PlantType"));
            var queryDeciduousType = selectCritList.Where(x => x.Characteristic.Contains("PlantType-Deciduous"));
            if (queryPlantType.Count() > 0 || queryDeciduousType.Count() > 0)
            {
               if (queryDeciduousType.Count() > 0)
                {
                    var plantTypeQuery = PredicateBuilder.False<WoodyPlant>();
                    var plantType = queryDeciduousType.ElementAt(0);


                    plantTypeQuery = (x => ((!x.family.Contains(plantType.SearchString1) && !x.family.Contains(plantType.SearchString2) && !x.family.Contains(plantType.SearchString3) && !x.family.Contains(plantType.SearchString4) && !x.family.Contains(plantType.SearchString5) && !x.family.Contains(plantType.SearchString6))));
                    
                    overallQuery = overallQuery.And(plantTypeQuery);
                }
               else
               {
                    var plantTypeQuery = PredicateBuilder.False<WoodyPlant>();
                    foreach (var plantType in queryPlantType)
                    {
                        plantTypeQuery = (x => x.family.Contains(plantType.SearchString1) || x.family.Contains(plantType.SearchString2) || x.family.Contains(plantType.SearchString3) || x.family.Contains(plantType.SearchString4) || x.family.Contains(plantType.SearchString5) || x.family.Contains(plantType.SearchString6));
                    }
                    overallQuery = overallQuery.And(plantTypeQuery);
                }
            }

            // Add selected Flower Color characteristics
            var queryLeafShape = selectCritList.Where(x => x.Characteristic.Contains("LeafShape"));
            if (queryLeafShape.Count() > 0)
            {
                var leafShapeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var leafShape in queryLeafShape) { leafShapeQuery = leafShapeQuery.Or(x => x.leafShape.ToLower().Contains(leafShape.SearchString1) || x.leafShape.ToLower().Contains(leafShape.SearchString2) || x.leafShape.ToLower().Contains(leafShape.SearchString3) || x.leafShape.ToLower().Contains(leafShape.SearchString4) || x.leafShape.ToLower().Contains(leafShape.SearchString5) || x.leafShape.ToLower().Contains(leafShape.SearchString6)); }
                overallQuery = overallQuery.And(leafShapeQuery);
            }

            var queryNeedleShape = selectCritList.Where(x => x.Characteristic.Contains("NeedleShape"));
            if (queryNeedleShape.Count() > 0)
            {
                var needleShapeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var needleShape in queryNeedleShape) { needleShapeQuery = needleShapeQuery.Or(x => x.leafShape.ToLower().Contains(needleShape.SearchString1) || x.leafShape.ToLower().Contains(needleShape.SearchString2)); }
                overallQuery = overallQuery.And(needleShapeQuery);
            }

            // Add selected Flower Color characteristics
            var queryLeafArrangement = selectCritList.Where(x => x.Characteristic.Contains("LeafArrangement"));
            if (queryLeafArrangement.Count() > 0)
            {
                var leafArrangementQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var leafArrangement in queryLeafArrangement) { leafArrangementQuery = leafArrangementQuery.Or(x => x.leafArrangement.ToLower().Contains(leafArrangement.SearchString1)); }
                overallQuery = overallQuery.And(leafArrangementQuery);
            }

            // Add selected Flower Color characteristics
            var queryTwigTexture = selectCritList.Where(x => x.Characteristic.Contains("TwigTexture"));
            if (queryTwigTexture.Count() > 0)
            {
                var twigTextureQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var twigTexture in queryTwigTexture)
                {
                    twigTextureQuery = twigTextureQuery.Or(x => x.twigTexture.ToLower().Contains(twigTexture.SearchString1) || x.twigTexture.ToLower().Contains(twigTexture.SearchString2) || x.twigTexture.ToLower().Contains(twigTexture.SearchString3) || x.twigTexture.ToLower().Contains(twigTexture.SearchString4) || x.twigTexture.ToLower().Contains(twigTexture.SearchString5) || x.twigTexture.ToLower().Contains(twigTexture.SearchString6) || x.twigTexture.ToLower().Contains(twigTexture.SearchString7) || x.twigTexture.ToLower().Contains(twigTexture.SearchString8) || x.twigTexture.ToLower().Contains(twigTexture.SearchString9));
                }
                overallQuery = overallQuery.And(twigTextureQuery);
            }

            // Add selected Flower Color characteristics
            var queryFlowerColor = selectCritList.Where(x => x.Characteristic.Contains("FlowerColor"));
            if (queryFlowerColor.Count() > 0)
            {
                var flowerColorQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var flowerColor in queryFlowerColor) { flowerColorQuery = flowerColorQuery.Or(x => x.flowerColor.ToLower().Contains(flowerColor.SearchString1) || x.flowerColor.ToLower().Contains(flowerColor.SearchString2) || x.flowerColor.ToLower().Contains(flowerColor.SearchString3)); }
                overallQuery = overallQuery.And(flowerColorQuery);
            }

            // Add selected Flower Color characteristics
            var queryBarkTexture = selectCritList.Where(x => x.Characteristic.Contains("BarkTexture"));
            if (queryBarkTexture.Count() > 0)
            {
                var barkTextureQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var barkTexture in queryBarkTexture)
                {
                    barkTextureQuery = barkTextureQuery.Or(x => x.barkTexture.ToLower().Contains(barkTexture.SearchString1) || x.barkTexture.ToLower().Contains(barkTexture.SearchString2) || x.barkTexture.ToLower().Contains(barkTexture.SearchString3) || x.barkTexture.ToLower().Contains(barkTexture.SearchString4) || x.barkTexture.ToLower().Contains(barkTexture.SearchString5) || x.barkTexture.ToLower().Contains(barkTexture.SearchString6) || x.barkTexture.ToLower().Contains(barkTexture.SearchString7) || x.barkTexture.ToLower().Contains(barkTexture.SearchString8) || x.barkTexture.ToLower().Contains(barkTexture.SearchString9) || x.barkTexture.ToLower().Contains(barkTexture.SearchString10));
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
                    flowerClusterQuery = flowerClusterQuery.Or(x => x.flowerCluster.ToLower().Contains(flowerCluster.SearchString1) || x.flowerCluster.ToLower().Contains(flowerCluster.SearchString2) || x.flowerCluster.ToLower().Contains(flowerCluster.SearchString3) || x.flowerCluster.ToLower().Contains(flowerCluster.SearchString4) || x.flowerCluster.ToLower().Contains(flowerCluster.SearchString5) || x.flowerCluster.ToLower().Contains(flowerCluster.SearchString6));
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
                    flowerShapeQuery = flowerShapeQuery.Or(x => x.flowerShape.ToLower().Contains(flowerShape.SearchString1) || x.flowerShape.ToLower().Contains(flowerShape.SearchString2) || x.flowerShape.ToLower().Contains(flowerShape.SearchString3) || x.flowerShape.ToLower().Contains(flowerShape.SearchString4) || x.flowerShape.ToLower().Contains(flowerShape.SearchString5) || x.flowerShape.ToLower().Contains(flowerShape.SearchString6));
                }
                overallQuery = overallQuery.And(flowerShapeQuery);
            }

            var queryFruitType = selectCritList.Where(x => x.Characteristic.Contains("FruitType"));
            if (queryFruitType.Count() > 0)
            {
                var fruitTypeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var fruitType in queryFruitType)
                {
                    fruitTypeQuery = fruitTypeQuery.Or(x => x.fruitType.ToLower().Contains(fruitType.SearchString1) || x.fruitType.ToLower().Contains(fruitType.SearchString2) || x.fruitType.ToLower().Contains(fruitType.SearchString3) || x.fruitType.ToLower().Contains(fruitType.SearchString4) || x.fruitType.ToLower().Contains(fruitType.SearchString5));
                }
                overallQuery = overallQuery.And(fruitTypeQuery);
            }

            var queryConeType = selectCritList.Where(x => x.Characteristic.Contains("ConeType"));
            if (queryConeType.Count() > 0)
            {
                var coneTypeQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var coneType in queryConeType)
                {
                    coneTypeQuery = coneTypeQuery.Or(x => x.fruitType.ToLower().Contains(coneType.SearchString1) && !x.fruitType.ToLower().Contains(coneType.SearchString2));
                }
                overallQuery = overallQuery.And(coneTypeQuery);
            }

            var queryFruitColor = selectCritList.Where(x => x.Characteristic.Contains("FruitColor"));
            if (queryFruitColor.Count() > 0)
            {
                var fruitColorQuery = PredicateBuilder.False<WoodyPlant>();
                foreach (var fruitColor in queryFruitColor)
                {
                    fruitColorQuery = fruitColorQuery.Or(x => x.fruitColor.ToLower().Contains(fruitColor.SearchString1) || x.fruitColor.ToLower().Contains(fruitColor.SearchString2));
                }
                overallQuery = overallQuery.And(fruitColorQuery);
            }

            return overallQuery;
        }

    }
}