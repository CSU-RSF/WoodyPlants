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
            conn.Insert(new WoodySearch() { Characteristic = "LeafType-Simple", Name = "Simple", Query = false, Column1 = "leafType", SearchString1 = "simple", IconFileName = "simple.png" });
            conn.Insert(new WoodySearch() { Characteristic = "LeafType-Spine", Name = "Spine", Query = false, Column1 = "leafType", SearchString1 = "spine"});
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Yellow", Name = "Yellow", Query = false, Column1 = "flowerColor", SearchString1 = "yellow"});
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Blue", Name = "Blue", Query = false, Column1 = "flowerColor", SearchString1 = "blue"});
            conn.Insert(new WoodySearch() { Characteristic = "FlowerColor-Red", Name = "Red", Query = false, Column1 = "flowerColor", SearchString1 = "red"});
        }
    }
}