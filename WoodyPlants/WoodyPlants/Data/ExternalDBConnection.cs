using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PortableApp.Models;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        // Declare variables
        public string Url = "http://sdt1.agsci.colostate.edu/mobileapi/api/woody";
        HttpClient client = new HttpClient();
        private string result;
        private Stream resultStream;

        // Set headers for client
        public ExternalDBConnection()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "p4OqMiplghVdWPbVv5rx84jdlskdJk*jdlsKDIE84");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        
        public async Task<IEnumerable<WoodyPlant>> GetAllPlants()
        {
            result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IList<WoodyPlant>>(result);
        }

        //public async Task<IEnumerable<WoodyGlossary>> GetAllTerms()
        //{
        //    result = await client.GetStringAsync(Url + "_glossary");
        //    return JsonConvert.DeserializeObject<IList<WoodyGlossary>>(result);
        //}

        public async Task<WoodySetting> GetDateUpdatedDataOnServer()
        {
            try
            {
                result = await client.GetStringAsync(Url + "_settings/DatePlantDataUpdatedOnServer");
            }
            catch(Exception e)
            {
                Debug.WriteLine("Error", e.Message);
                return null;
            }
            return JsonConvert.DeserializeObject<WoodySetting>(result);
        }

        //hardcoded
        public async Task<IEnumerable<WoodySetting>> GetImageZipFileSettings()
        {
            result = await client.GetStringAsync(Url + "_settings/ImagesZipFile");
            WoodySetting setting = JsonConvert.DeserializeObject<WoodySetting>(result);

            List<WoodySetting> settingList = new List<WoodySetting>();

            settingList.Add(setting);

            return settingList;
        }
          

        public async Task<Stream> GetImageZipFiles(string imageFileToDownload)
        {
            resultStream = await client.GetStreamAsync(Url + "/image_zip_files/" + imageFileToDownload);
            return resultStream;
        }

    }
}
