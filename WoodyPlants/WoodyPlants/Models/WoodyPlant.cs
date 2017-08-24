using PCLStorage;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PortableApp.Models
{
    [Table("woody_plants")]
    public class WoodyPlant
    {
        [PrimaryKey]        
        public int plant_id { get; set; }

        [Unique]
        public int plant_imported_id { get; set; }
        
        public string family { get; set; }
        
        public string scientificNameWeber { get; set; }
        
        public string leafType { get; set; }
        
        public string leafShape { get; set; }
        
        public string growthForm { get; set; }
        
        public string growthDuration { get; set; }
        
        public string scientificNameOther { get; set; }
        
        public string commonName { get; set; }
        
        public string commonNameSecondary { get; set; }
        
        public string scientificNameMeaningWeber { get; set; }
        
        public string plantClass { get; set; }
        
        public string plantSubClass { get; set; }
        
        public string origin { get; set; }
        
        public string weedManagement { get; set; }
        
        public string edibility { get; set; }
        
        public string livestock { get; set; }
        
        public string toxicity { get; set; }
        
        public string ecologicalRelationships { get; set; }
        
        public string frequency { get; set; }
        
        public string habitat { get; set; }
        
        public string scientificNameNelson { get; set; }
        
        public string scientificNameMeaningNelson { get; set; }
        
        public string seasonOfBloom { get; set; }
        
        public string familyCharacteristics { get; set; }
        
        public string flowerSymmetry { get; set; }
        
        public string flowerCluster { get; set; }
        
        public string flowerShape { get; set; }
        
        public string commonNameDerivation { get; set; }
        
        public string landscapingCultivar { get; set; }
        
        public string flowerColor { get; set; }
        
        public string fruitColor { get; set; }
        
        public string availability { get; set; }
        
        public string keyCharacteristics { get; set; }
        
        public string lifeZone { get; set; }
        
        public string endemicLocation { get; set; }
        
        public string landscapingUse { get; set; }
        
        public string matureHeight { get; set; }
        
        public string matureSpread { get; set; }
        
        public string lightRequirements { get; set; }
        
        public string soilRequirements { get; set; }
        
        public string fiber { get; set; }
        
        public string otherInformation { get; set; }
        
        public string flowerSize { get; set; }
        
        public string petalNumber { get; set; }
        
        public string flowerStructure { get; set; }
        
        public string moistureRequirements { get; set; }
        
        public string pronunciation { get; set; }
        
        public string fruitType { get; set; }
        
        public string subspecies { get; set; }
        
        public string variety { get; set; }
        
        public string forma { get; set; }
        
        public string legalStatus { get; set; }
        
        public string guanellaPass { get; set; }
        
        public string plainCc { get; set; }
        
        public string noNameCreek { get; set; }
        
        public string maloitPark { get; set; }
        
        public string vailNc { get; set; }
        
        public string lovelandPass { get; set; }
        
        public string roxborough { get; set; }
        
        public string castlewood { get; set; }
        
        public string custerCounty { get; set; }
        
        public string dbg { get; set; }
        
        public string grassesAtGreenMtn { get; set; }
        
        public string eastPortal { get; set; }
        
        public string mesaCounty { get; set; }
        
        public string tellerCounty { get; set; }
        
        public string goldenGate { get; set; }
        
        public string southPlattePark { get; set; }
        
        public string greenMt { get; set; }
        
        public string reynolds { get; set; }
        
        public string grassesManual { get; set; }
        
        public string falcon { get; set; }
        
        public string lookoutMt { get; set; }
        
        public string southValley { get; set; }
        
        public string deerCreek { get; set; }
        
        public string lairOTheBear { get; set; }
        
        public string print { get; set; }
        
        public string highPlains { get; set; }
        
        public string shrubs { get; set; }


        public bool isFavorite { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<WoodyPlantImage> Images { get; set; }

        //public IFolder rootFolder { get { return FileSystem.Current.LocalStorage; } }
        public ImageSource ThumbnailPath { get { return ImageSource.FromResource("WoodyPlants.Resources.Images.image.png"); } }
    }

}
