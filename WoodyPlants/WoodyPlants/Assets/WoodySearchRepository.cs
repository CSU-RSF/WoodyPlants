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
            conn.Insert(new WoodySearch() { Characteristic = "PlantType-Deciduous", Name = "Deciduous", Query = false, Column1 = "family", SearchString1 = "Pine", SearchString2 = "Cypress", SearchString3 = "Grape", SearchString4 = "Buttercup", SearchString5 = "Barberry", SearchString6 = "Cactus", IconFileName = "Deciduous.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "PlantType-Conifer", Name = "Conifer", Query = false, Column1 = "family", SearchString1 = "Pine", SearchString2 = "Cypress", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Conifer.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "PlantType-Vine", Name = "Vine", Query = false, Column1 = "family", SearchString1 = "Grape", SearchString2 = "Buttercup", SearchString3 = "Barberry", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Vine.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "PlantType-Cacti", Name = "Cacti", Query = false, Column1 = "family", SearchString1 = "Cactus", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Cacti.jpg" });

            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Narrow", Name = "Narrow", Query = false, Column1 = "leafShape", SearchString1 = "narrow", SearchString2 = "linear", SearchString3 = "lanceolate", SearchString4 = "oblong", SearchString5 = "elliptic", SearchString6 = "oval", IconFileName = "narrow.png"});
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Deltoid", Name = "Deltoid", Query = false, Column1 = "leafShape", SearchString1 = "deltoid", SearchString2 = "ovate", SearchString3 = "triangular", SearchString4 = "cordate", SearchString5 = "null", SearchString6 = "null", IconFileName = "Deltoid.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Orbicular", Name = "Orbicular", Query = false, Column1 = "leafShape", SearchString1 = "orbicular", SearchString2 = "reniform", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Orbicular.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Oblanceolate", Name = "Oblanceolate", Query = false, Column1 = "leafShape", SearchString1 = "oblanceolate", SearchString2 = "obovate", SearchString3 = "spatulate", SearchString4 = "cuneate", SearchString5 = "null", SearchString6 = "null", IconFileName = "oblanceolate.png" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Palmately", Name = "Palmately", Query = false, Column1 = "leafShape", SearchString1 = "palmately", SearchString2 = "lobed", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Palmate.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Lobed", Name = "Lobed", Query = false, Column1 = "leafShape", SearchString1 = "lobed", SearchString2 = "lyrate", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "LobedCombined.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafShape-Pinnate", Name = "Pinnate", Query = false, Column1 = "leafShape", SearchString1 = "pinnate", SearchString2 = "palmate", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "PinnateCombined.jpg" });

            //HERE
            conn.Insert(new WoodySearch() { Characteristic = "ShapeVineLeaf-Shape1", Name = "Simple", Query = false, Column1 = "leafType", SearchString1 = "simple", SearchString2 = "null", SearchString3 = "null", IconFileName = "SimpleLeaf.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "ShapeVineLeaf-Shape2", Name = "Compound", Query = false, Column1 = "leafType", SearchString1 = "compound", SearchString2 = "pinnate", SearchString3 = "bipinnate", IconFileName = "CompoundLeaf.jpg" });

            //HERE
            conn.Insert(new WoodySearch() { Characteristic = "NeedleShape-TwoCluster", Name = "Clusters of 2-3", Query = false, Column1 = "leafShape", SearchString1 = "fascicles of 2", SearchString2 = "null", IconFileName = "Clusterof2.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "NeedleShape-FiveCluster", Name = "Clusters of 5", Query = false, Column1 = "leafShape", SearchString1 = "fascicles of 5", SearchString2 = "null", IconFileName = "Clusterof5.jpg"});
            conn.Insert(new WoodySearch() { Characteristic = "NeedleShape-Flat", Name = "Flat", Query = false, Column1 = "leafShape", SearchString1 = "flat", SearchString2 = "null", IconFileName = "FlatNeedle.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "NeedleShape-Sharp", Name = "Sharp", Query = false, Column1 = "leafShape", SearchString1 = "sharp", SearchString2 = "awl", IconFileName = "Sharp.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "NeedleShape-Scale", Name = "Scale", Query = false, Column1 = "leafShape", SearchString1 = "scale", SearchString2 = "null", IconFileName = "Scale.jpg" });

            //HERE
            conn.Insert(new WoodySearch() { Characteristic = "ConeType-SolidCone", Name = "Solid Woody Cone", Query = false, Column1 = "fruitType", SearchString1 = "woody", SearchString2 = "null", IconFileName = "SolidCone.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "ConeType-PaperyCone", Name = "Papery Cone", Query = false, Column1 = "fruitType", SearchString1 = "papery", SearchString2 = "null", IconFileName = "PaperyCone.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "ConeType-Berry", Name = "Berry", Query = false, Column1 = "fruitType", SearchString1 = "berry", SearchString2 = "berries", IconFileName = "Berry.jpg" });                   

            conn.Insert(new WoodySearch() { Characteristic = "LeafArrangement-Alternate", Name = "Alternate", Query = false, Column1 = "leafArrangement", SearchString1 = "alternate", IconFileName = "Alternate.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafArrangement-Opposite", Name = "Opposite", Query = false, Column1 = "leafArrangement", SearchString1 = "opposite", IconFileName = "Opposite.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafArrangement-Whorled", Name = "Whorled", Query = false, Column1 = "leafArrangement", SearchString1 = "whorled", IconFileName = "Whorled.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafArrangement-Basal", Name = "Basal", Query = false, Column1 = "leafArrangement", SearchString1 = "basal", IconFileName = "Basal.jpg" });

            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Hairy", Name = "Hairy", Query = false, Column1 = "twigTexture", SearchString1 = "hairy", SearchString2 = "pubescent", SearchString3 = "villose", SearchString4 = "tomentose", SearchString5 = "pilose", SearchString6 = "woolly", SearchString7 = "canescent", SearchString8 = "velvety", SearchString9 = "fuzz" });
            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Smooth", Name = "Smooth", Query = false, Column1 = "twigTexture", SearchString1 = "smooth", SearchString2 = "glabrous", SearchString3 = "waxy", SearchString4 = "glaucous", SearchString5 = "shiny", SearchString6 = "powdery", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Rough", Name = "Rough", Query = false, Column1 = "twigTexture", SearchString1 = "rough", SearchString2 = "lenticellate", SearchString3 = "warty", SearchString4 = "blistered", SearchString5 = "scabrous", SearchString6 = "scaly", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Peeling", Name = "Peeling", Query = false, Column1 = "twigTexture", SearchString1 = "peeling", SearchString2 = "shreddy", SearchString3 = "shredding", SearchString4 = "flaky", SearchString5 = "flake", SearchString6 = "null", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Thorny", Name = "Thorny", Query = false, Column1 = "twigTexture", SearchString1 = "thorny", SearchString2 = "bristly", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null" });
            conn.Insert(new WoodySearch() { Characteristic = "TwigTexture-Sticky", Name = "Sticky", Query = false, Column1 = "twigTexture", SearchString1 = "sticky", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", SearchString7 = "null", SearchString8 = "null", SearchString9 = "null" });

            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Yellow", Name = "Yellow", Query = false, Column1 = "flowerColor", SearchString1 = "yellow", SearchString2 = "null", SearchString3 = "null"});
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Blue", Name = "Blue", Query = false, Column1 = "flowerColor", SearchString1 = "blue", SearchString2 = "null", SearchString3 = "null"});
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

            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Inconspicuous", Name = "Inconspicuous", Query = false, Column1 = "flowerSymmetry", SearchString1 = "inconspicuous", SearchString2 = "apetalous", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Inconspicuous.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Round", Name = "Round", Query = false, Column1 = "flowerSymmetry", SearchString1 = "round", SearchString2 = "composite", SearchString3 = "rotate", SearchString4 = "cylindrical", SearchString5 = "null", SearchString6 = "null", IconFileName = "Round.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-BellShaped", Name = "Bell-Shaped", Query = false, Column1 = "flowerSymmetry", SearchString1 = "bell-shaped", SearchString2 = "campanulate", SearchString3 = "funnelform", SearchString4 = "salverform", SearchString5 = "tubular", SearchString6 = "urceolate", IconFileName = "Bell.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-CupShaped", Name = "Cup-Shaped ", Query = false, Column1 = "flowerSymmetry", SearchString1 = "cup-shaped", SearchString2 = "crateriform", SearchString3 = "cupuliform", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null", IconFileName = "Cup.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-StarShaped", Name = "Star Or Cross Shaped", Query = false, Column1 = "flowerSymmetry", SearchString1 = "star-shaped", SearchString2 = "cross-shaped", SearchString3 = "cruciform", SearchString4 = "stellate", SearchString5 = "null", SearchString6 = "null", IconFileName = "Star.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FlowerShape-Other", Name = "Other", Query = false, Column1 = "flowerSymmetry", SearchString1 = "papilionaceous", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", SearchString6 = "null" });

            //conn.Insert(new WoodySearch() { Characteristic = "FlowerVineShape-Shape1", Name = "Flower Shape One", Query = false, Column1 = "flowerShape", SearchString1 = "null"});
            //conn.Insert(new WoodySearch() { Characteristic = "FlowerVineShape-Shape2", Name = "Flower Shape Two", Query = false, Column1 = "flowerShape", SearchString1 = "null"});
            //conn.Insert(new WoodySearch() { Characteristic = "FlowerVineShape-Shape3", Name = "Flower Shape Three", Query = false, Column1 = "flowerShape", SearchString1 = "null"});

            conn.Insert(new WoodySearch() { Characteristic = "FruitType-DrySeed", Name = "Dry Seed", Query = false, Column1 = "familyCharacteristics", SearchString1 = "achene", SearchString2 = "cypselae", SearchString3 = "utricle", SearchString4 = "null", SearchString5 = "null", IconFileName = "Achene.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Acorn", Name = "Acorn", Query = false, Column1 = "familyCharacteristics", SearchString1 = "nut", SearchString2 = "nutlet", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", IconFileName = "Acorn.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Fleshy", Name = "Fleshy", Query = false, Column1 = "familyCharacteristics", SearchString1 = "berry", SearchString2 = "pome", SearchString3 = "drupe", SearchString4 = "hip", SearchString5 = "drupelet", IconFileName = "Fleshy.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Cone", Name = "Cone", Query = false, Column1 = "familyCharacteristics", SearchString1 = "cone", SearchString2 = "cone-like", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", IconFileName = "Cone.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Capsule", Name = "Capsule", Query = false, Column1 = "familyCharacteristics", SearchString1 = "legume", SearchString2 = "pod", SearchString3 = "follicle", SearchString4 = "loment", SearchString5 = "schizocarp", IconFileName = "Capsule.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "FruitType-Samara", Name = "Samara", Query = false, Column1 = "familyCharacteristics", SearchString1 = "samara", SearchString2 = "null", SearchString3 = "null", SearchString4 = "null", SearchString5 = "null", IconFileName = "Samara.jpg" });       

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

            conn.Insert(new WoodySearch() { Characteristic = "CactusShape-Flat1", Name = "Flat", Query = false, Column1 = "twigTexture", SearchString1 = "flat", IconFileName = "Flat.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "CactusShape-Sphere", Name = "Sphere", Query = false, Column1 = "twigTexture", SearchString1 = "spheric", IconFileName = "Sphere.jpg" });                
            conn.Insert(new WoodySearch() { Characteristic = "CactusShape-Branched", Name = "Branched", Query = false, Column1 = "twigTexture", SearchString1 = "branched", IconFileName = "Branched.jpg" });
            conn.Insert(new WoodySearch() { Characteristic = "CactusShape-Cylinder", Name = "Cylinder", Query = false, Column1 = "twigTexture", SearchString1 = "cylindric", IconFileName = "Cylinder.jpg" });


        }
    
    }
}