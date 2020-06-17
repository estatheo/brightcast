using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using brightcast.Entities;
using brightcast.Helpers;
using brightcast.Models.Campaigns;
using brightcast.Models.ContactLists;
using brightcast.Models.Contacts;
using brightcast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace brightcast.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FileController(
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [HttpPost("uploadImage"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImage(string name)
        {
            var url = "";
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    // Create Reference to Azure Storage Account
                    String strorageconn = _appSettings.StorageConnectionString;
                    CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

                    //Create Reference to Azure Blob
                    CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference("images");

                    await container.CreateIfNotExistsAsync();

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(name + "-" + file.FileName);
                    if (url == "")
                    {
                        url += blockBlob.Uri.AbsoluteUri;
                    }
                    else
                    {
                        url += "_" + blockBlob.Uri.AbsoluteUri;
                    }
                    using (var filestream = file.OpenReadStream())
                    {

                        await blockBlob.UploadFromStreamAsync(filestream);

                    }
                }
                return Ok(new { name = url });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Upload Failed: " + ex.Message);
            }
        }

        [HttpPost("uploadDoc"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadDoc(string name)
        {
            var url = "";
            try
            {
                foreach (var file in Request.Form.Files)
                {
                    // Create Reference to Azure Storage Account
                    String strorageconn = _appSettings.StorageConnectionString;
                    CloudStorageAccount storageacc = CloudStorageAccount.Parse(strorageconn);

                    //Create Reference to Azure Blob
                    CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

                    CloudBlobContainer container = blobClient.GetContainerReference("docs");

                    await container.CreateIfNotExistsAsync();

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(name + "-" + file.FileName);
                    if (url == "")
                    {
                        url += blockBlob.Uri.AbsoluteUri;
                    }
                    else
                    {
                        url += "_" + blockBlob.Uri.AbsoluteUri;
                    }
                    using (var filestream = file.OpenReadStream())
                    {

                        await blockBlob.UploadFromStreamAsync(filestream);

                    }
                }
                return Ok(new { name = url });
            }
            catch (System.Exception ex)
            {
                return BadRequest("Upload Failed: " + ex.Message);
            }
        }


    }
}
