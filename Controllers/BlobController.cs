using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureLabStorageProject.Models;
using AzureLabStorageProject.Repository;
using StorageCURD.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace AzureLabStorageProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobStorage _blobStorage;
        private readonly IConfiguration _configuration;
        public BlobController(IBlobStorage blobStorage, IConfiguration configuration)
        {
            _blobStorage = blobStorage;
            _configuration = configuration;
        }

      


        [HttpGet]
        [Route("GetFile")]
        public async Task<AZCustomeResponse> GetFile()
        {
            AZCustomeResponse response = new AZCustomeResponse();
            response.Result = await _blobStorage.ListAsync();

            return response;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<AZCustomeResponse> Upload(IFormFile file)
        {
            AZCustomeResponse response = new AZCustomeResponse();
             response.Result = await _blobStorage.UploadAsync(file);

            return response;
        }

        //[HttpGet]
        //[Route("Download")]
        //public async Task<IActionResult> Download(string filename)
        //{
        //    BlobDto? blobObject = await _blobStorage.DownloadAsync(filename);

        //    if (blobObject == null)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
        //    }
        //    else
        //    {
        //        return File(blobObject.Content, blobObject.ContentType, blobObject.Name);
        //    }
           
        //}

        [HttpDelete]
        [Route("Delete")]
        public async Task<AZCustomeResponse> Delete(string filename)
        {
            AZCustomeResponse response = new AZCustomeResponse();            
            response.Result = await _blobStorage.DeleteAsync(filename);
            return response;

        }

        
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            CloudBlockBlob blockBlob;
            await using (MemoryStream memoryStream = new MemoryStream())
            {
                string blobstorageconnection = _configuration.GetConnectionString("StorageConnectionString");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_configuration.GetConnectionString("StorageContainerName"));
                blockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                await blockBlob.DownloadToStreamAsync(memoryStream);
            }
            Stream blobStream = blockBlob.OpenReadAsync().Result;
            return File(blobStream, blockBlob.Properties.ContentType, blockBlob.Name);
        }
    }
}
