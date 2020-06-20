using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using brightcast.Entities;
using brightcast.Models.Contacts;
using CsvHelper;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace brightcast.Helpers
{
    public class CsvParser
    {

        private readonly AppSettings _appSettings;

        public CsvParser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<List<ContactParseModel>> ParseFile(string fileUrl)
        {
            // Create Reference to Azure Storage Account
            String strorageconn = _appSettings.StorageConnectionString;
            CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

            //Create Reference to Azure Blob
            CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("docs");

            await container.CreateIfNotExistsAsync();

            //The next 7 lines upload the file test.txt with the name DemoBlob on the container "democontainer"
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileUrl.Split('/').ToList().Last());

            using (var filestream = await blockBlob.OpenReadAsync())
            {
                using (var reader = new StreamReader(filestream))
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csvReader.GetRecords<ContactParseModel>().ToList();
                    return records;
                }
            }
            
        }

    }
}
