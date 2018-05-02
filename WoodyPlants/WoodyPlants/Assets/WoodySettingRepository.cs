using System.Collections.Generic;
using System.Linq;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;

namespace PortableApp
{

    public class WoodySettingRepository : DBConnection
	{

        public string StatusMessage { get; set; }

        public WoodySettingRepository()
		{
            // Create the Woody Setting table (only if it's not yet created)
            //conn.DropTable<WoodySetting>();
            conn.CreateTable<WoodySetting>();
            SeedDB();
        }

        public void ClearWoodySettings()
        {
            conn.DropTable<WoodySetting>();
            conn.CreateTable<WoodySetting>();
            SeedDB();
        }

        // return a list of Woodyplants saved to the WoodySetting table in the database
        public List<WoodySetting> GetAllWoodySettings()
        {
            return (from s in conn.Table<WoodySetting>() select s).ToList();
        }

        // get a list of image settings stored in the local database
        public List<WoodySetting> GetAllImageSettings()
        {
            return conn.Table<WoodySetting>().Where(s => s.name.Equals("ImagesZipFile")).ToList();
        }

        // get an individual setting based on its name
        public WoodySetting GetSetting(string settingName)
        {
            return conn.Table<WoodySetting>().FirstOrDefault(s => s.name.Equals(settingName));
        }

        // (async) get an individual setting based on its name
        public async Task<WoodySetting> GetSettingAsync(string settingName)
        {
            return await connAsync.Table<WoodySetting>().Where(s => s.name.Equals(settingName)).FirstOrDefaultAsync();
        }

        public WoodySetting GetImageZipFileSetting(string fileName)
        {
            return conn.Table<WoodySetting>().Where(s => s.valuetext.Equals(fileName)).FirstOrDefault();
        }

        // add a setting
        public void AddSetting(WoodySetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.Insert(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add a setting async
        public async Task AddSettingAsync(WoodySetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public void AddOrUpdateSetting(WoodySetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = conn.InsertOrReplace(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }

        }

        // add or update a setting
        public async Task AddOrUpdateSettingAsync(WoodySetting setting)
        {
            try
            {
                if (string.IsNullOrEmpty(setting.name))
                    throw new Exception("Valid setting name required");

                var result = await connAsync.InsertOrReplaceAsync(setting);
                StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, setting);
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Failed to add/update {0}. Error: {1}", setting, ex.Message);
            }
            
        }

        // Seed database with essential settings
        public void SeedDB()
        {
            if (GetSetting("Sort Field") == null)
                conn.Insert(new WoodySetting { name = "Sort Field", valuetext = "Scientific Name", valueint = 0 });
            if (GetSetting("Download Images") == null)
                conn.Insert(new WoodySetting { name = "Download Images", valuebool = true });
            if (GetSetting("Date Plants Downloaded") == null)
                conn.Insert(new WoodySetting { name = "Date Plants Downloaded", valuetimestamp = null });
        }

    }
}