using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WoodySearchRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WoodySearchRepository()
		{

            conn.DropTable<WoodySearch>();

            // Create the Woody Search table (only if it's not yet created) and seed it if needed
            conn.CreateTable<WoodySearch>();
            SeedDB(); 
		}

        // return a list of all Woody Search Criteria
        public List<WoodySearch> GetAllWoodySearchCriteria()
        {
            return conn.Table<WoodySearch>().ToList();
        }

        // return a list of only selected (for querying) search criteria
        public List<WoodySearch> GetQueryableSearchCriteria()
        {
            return conn.Table<WoodySearch>().Where(x => x.Query == true).ToList();
        }

        // return a list of only selected (for querying) search criteria
        public async Task<List<WoodySearch>> GetQueryableSearchCriteriaAsync()
        {
            return await connAsync.Table<WoodySearch>().Where(x => x.Query == true).ToListAsync();
        }

        // get an individual search criteria based on its name
        public async Task<WoodySearch> GetSearchAsync(string name)
        {
            return await connAsync.Table<WoodySearch>().Where(s => s.Name.Equals(name)).FirstOrDefaultAsync();
        }

        // update a search criteria
        public async Task UpdateSearchCriteriaAsync(WoodySearch criteria)
        {
            try
            {
                var result = await connAsync.UpdateAsync(criteria);
                StatusMessage = string.Format("{0} record(s) updated [Name: {1})", result, criteria);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", criteria, ex.Message);
            }            
        }

        // Seed database with Search Criteria
        public void SeedDB()
        {
           // conn.Insert(new WoodySearch() { Characteristic = "LeafType-Simple", Name = "Simple", Query = false, Column1 = "leafType", SearchString1 = "simple", IconFileName = "simple.png" });
            //conn.Insert(new WoodySearch() { Characteristic = "LeafType-Spine", Name = "Spine", Query = false, Column1 = "leafType", SearchString1 = "spine"});

            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Yellow", Name = "Yellow", Query = false, Column1 = "flowerColor", SearchString1 = "yellow", SearchString2 = "null", SearchString3 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Blue", Name = "Blue", Query = false, Column1 = "flowerColor", SearchString1 = "blue", SearchString2 = "null", SearchString3 = "null", IconFileName = "simple.png" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Brown", Name = "Brown", Query = false, Column1 = "flowerColor", SearchString1 = "brown", SearchString2 = "tan", SearchString3 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-White", Name = "White", Query = false, Column1 = "flowerColor", SearchString1 = "white", SearchString2 = "cream", SearchString3 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Green", Name = "Green", Query = false, Column1 = "flowerColor", SearchString1 = "green", SearchString2 = "null", SearchString3 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Red", Name = "Red", Query = false, Column1 = "flowerColor", SearchString1 = "red", SearchString2 = "maroon", SearchString3 = "rose" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Orange", Name = "Orange", Query = false, Column1 = "flowerColor", SearchString1 = "orange", SearchString2 = "null", SearchString3 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Pink", Name = "Pink", Query = false, Column1 = "flowerColor", SearchString1 = "pink", SearchString2 = "null", SearchString3 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Purple", Name = "Purple", Query = false, Column1 = "flowerColor", SearchString1 = "purple", SearchString2 = "violet", SearchString3 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "BarkTexture-Smooth", Name = "Smooth", Query = false, Column1 = "barkDescription", SearchString1 = "glabrous", SearchString2 = "smooth", SearchString3 = "corky", SearchString4 = "waxy", SearchString5 = "spongy", SearchString6 = "null", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null", SearchString10 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "BarkTexture-Bumpy", Name = "Bumpy", Query = false, Column1 = "barkDescription", SearchString1 = "bumpy", SearchString2 = "lenticellate", SearchString3 = "blistered", SearchString4 = "warty", SearchString5 = "null", SearchString6 = "null", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null", SearchString10 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "BarkTexture-Peeling", Name = "Peeling", Query = false, Column1 = "barkDescription", SearchString1 = "peeling", SearchString2 = "shreddy", SearchString3 = "shredding", SearchString4 = "flaky", SearchString5 = "flake", SearchString6 = "fibrous", SearchString7 = "exfoliating", SearchString8 = "scale", SearchString9 = "scaly", SearchString10 = "splitting" });
            conn.Insert(new WoodySearch() { Characteristic = "BarkTexture-Furrowed", Name = "Furrowed", Query = false, Column1 = "barkDescription", SearchString1 = "furrowed", SearchString2 = "fissured", SearchString3 = "plated", SearchString4 = "ridges", SearchString5 = "grooved", SearchString6 = "netted", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null", SearchString10 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "FlowerCluster-Dense", Name = "Dense", Query = false, Column1 = "flowerSize", SearchString1 = "corymb", SearchString2 = "capitate", SearchString3 = "glomerule", SearchString4 = "umbel", SearchString5 = "strobilus", SearchString6 = "cyme" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerCluster-Loose", Name = "Loose", Query = false, Column1 = "flowerSize", SearchString1 = "panicle", SearchString2 = "raceme", SearchString3 = "spike", SearchString4 = "paniculiform ", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerCluster-Solitary", Name = "Solitary", Query = false, Column1 = "flowerSize", SearchString1 = "solitary", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerCluster-Catkin", Name = "Catkin", Query = false, Column1 = "flowerSize", SearchString1 = "catkin", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Inconspicuous", Name = "Inconspicuous", Query = false, Column1 = "flowerSymmetry", SearchString1 = "inconspicuous", SearchString2 = "apetalous", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Round", Name = "Round", Query = false, Column1 = "flowerSymmetry", SearchString1 = "round", SearchString2 = "composite", SearchString3 = "rotate", SearchString4 = "cylindrical", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-BellShaped", Name = "Bell-Shaped", Query = false, Column1 = "flowerSymmetry", SearchString1 = "bell-shaped", SearchString2 = "campanulate", SearchString3 = "funnelform", SearchString4 = "salverform", SearchString5 = "tubular", SearchString6 = "urceolate" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-CupShaped", Name = "Cup-Shaped ", Query = false, Column1 = "flowerSymmetry", SearchString1 = "cup-shaped", SearchString2 = "crateriform", SearchString3 = "cupuliform", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-StarShaped", Name = "Star-Shaped", Query = false, Column1 = "flowerSymmetry", SearchString1 = "star-shaped", SearchString2 = "cross-shaped", SearchString3 = "cruciform", SearchString4 = "stellate", SearchString5 = "null", SearchString6 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Other", Name = "Other", Query = false, Column1 = "flowerSymmetry", SearchString1 = "papilionaceous", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "FruitType-DrySeed", Name = "Dry Seed", Query = false, Column1 = "familyCharacteristics", SearchString1 = "achene", SearchString2 = "cypselae", SearchString3 = "utricle", SearchString4 = "null", SearchString5 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Acorn", Name = "Acorn", Query = false, Column1 = "familyCharacteristics", SearchString1 = "nut", SearchString2 = "nutlet", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Fleshy", Name = "Fleshy", Query = false, Column1 = "familyCharacteristics", SearchString1 = "berry", SearchString2 = "pome", SearchString3 = "drupe", SearchString4 = "hip", SearchString5 = "drupelet" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Cone", Name = "Cone", Query = false, Column1 = "familyCharacteristics", SearchString1 = "cone", SearchString2 = "cone-like", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Capsule", Name = "Capsule", Query = false, Column1 = "familyCharacteristics", SearchString1 = "legume", SearchString2 = "pod", SearchString3 = "follicle", SearchString4 = "loment", SearchString5 = "schizocarp" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Samara", Name = "Samara", Query = false, Column1 = "familyCharacteristics", SearchString1 = "samara", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Yellow", Name = "Yellow", Query = false, Column1 = "fruitType", SearchString1 = "yellow", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Blue", Name = "Blue", Query = false, Column1 = "fruitType", SearchString1 = "blue", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Brown", Name = "Brown", Query = false, Column1 = "fruitType", SearchString1 = "brown", SearchString2 = "tan"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-White", Name = "White", Query = false, Column1 = "fruitType", SearchString1 = "white", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Green", Name = "Green", Query = false, Column1 = "fruitType", SearchString1 = "green", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Red", Name = "Red", Query = false, Column1 = "fruitType", SearchString1 = "red", SearchString2 = "pink"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Orange", Name = "Orange", Query = false, Column1 = "fruitType", SearchString1 = "orange", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Black", Name = "Black", Query = false, Column1 = "fruitType", SearchString1 = "black", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Purple", Name = "Purple", Query = false, Column1 = "fruitType", SearchString1 = "purple", SearchString2 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FruitColor-Gray", Name = "Gray", Query = false, Column1 = "fruitType", SearchString1 = "gray", SearchString2 = "silver"});
        }
    
    }
}