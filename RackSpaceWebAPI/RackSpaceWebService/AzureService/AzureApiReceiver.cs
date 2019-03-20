using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace RackSpaceWebService.AzureService
{
    public class AzureApiReceiver
    {
        private string AZURE_API_URL;
        public AzureApiReceiver()
        {
            AZURE_API_URL = ConfigurationManager.AppSettings["RMXWebService"];
        }

        public string GetVehicleKeyWord(Guid key)
        {
            var response = GetResponseFromAzureWebAPI("RackSpace", key);
            if (response == null)
                return null;
            return response.Content.ReadAsAsync<string>().Result;
        }

        private HttpResponseMessage GetResponseFromAzureWebAPI(string method, Guid param)
        {
            try
            {
                var client = new HttpClient() { BaseAddress = new Uri(AZURE_API_URL) };
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(method + "/" + param).Result;
                if (response.IsSuccessStatusCode)
                    return response;
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}