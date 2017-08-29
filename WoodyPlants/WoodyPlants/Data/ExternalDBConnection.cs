using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PortableApp.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Diagnostics;

namespace PortableApp
{
    public class ExternalDBConnection
    {
        // Declare variables
        public string Url = "http://sdt1.cas.colostate.edu/mobileapi/api/woody";
        //public string Url = (Debugger.IsAttached == true) ? "http://129.82.38.57:61045/api/woody" : "http://sdt1.cas.colostate.edu/mobileapi/api/woody";
        HttpClient client = new HttpClient();
        private string result;
        private Stream resultStream;

        // Set headers for client
        public ExternalDBConnection()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", "p4OqMiplghVdWPlol5rx84jdlskdJk*jdlsKDIE42");
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
            catch
            {
                return null;
            }
            return JsonConvert.DeserializeObject<WoodySetting>(result);
        }

        //public async Task<IEnumerable<WoodySetting>> GetImageZipFileSettings()
        //{
        //    result = await client.GetStringAsync(Url + "_settings/images");
        //    return JsonConvert.DeserializeObject<IList<WoodySetting>>(result);
        //}

        //public async Task<Stream> GetImageZipFiles(string imageFileToDownload)
        //{
        //    resultStream = await client.GetStreamAsync(Url + "/image_zip_files/" + imageFileToDownload);
        //    return resultStream;
        //}

    }
}
