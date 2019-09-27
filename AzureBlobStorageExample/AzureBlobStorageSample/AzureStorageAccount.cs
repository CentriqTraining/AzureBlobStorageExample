using Microsoft.Azure.KeyVault;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStorageSample
{
    public static class AzureStorageAccount
    {
        public static string GetConnectionString()
        {
            string ConfigFileContentString = File.ReadAllText("../../Configuration/config.json");
            var Config = JsonConvert.DeserializeObject<Dictionary<string, string>>(ConfigFileContentString);
            return Config["connectionString"];
        }
    }
}
