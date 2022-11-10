using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using AzureLabStorageProject.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;

namespace AzureLabStorageProject.Models
{
    public class BlobStorageService : IBlobStorage
    {
        public IConfiguration _configuration;
        public string  StorageConnectionString=string.Empty;
        public string StorageContainerName = string.Empty;
        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
            StorageConnectionString = _configuration.GetConnectionString("StorageConnectionString");
            StorageContainerName = _configuration.GetConnectionString("StorageContainerName");

        }

        /// <summary>
        /// DeleteAsync 
        /// </summary>
        /// <param name="blobFilename"></param>
        /// <returns></returns>
        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient( StorageConnectionString,StorageContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {

                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
        }

        /// <summary>
        /// DownloadAsync
        /// </summary>
        /// <param name="blobFilename"></param>
        /// <returns></returns>
        public async Task<BlobDto> DownloadAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(StorageConnectionString, StorageContainerName);
            BlobClient file = client.GetBlobClient(blobFilename);
                BlobDto blob = new BlobDto();
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();

                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                blob.Content = blobContent;
                blob.Name = name;
                blob.ContentType = contentType ;
                }

            return blob;

        }

        /// <summary>
        /// ListAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<BlobDto>> ListAsync()
        {
            BlobContainerClient blobContainer = new BlobContainerClient(StorageConnectionString, StorageContainerName);
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
           
                string uri = blobContainer.Uri.ToString();
                var name = blobItem.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = blobItem.Properties.ContentType
                });
            }

            return files;
        }

        /// <summary>
        /// UploadAsync
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<BlobProperties> UploadAsync(IFormFile file)
        {
            BlobResponseDto response = new BlobResponseDto();
            BlobContainerClient client = new BlobContainerClient(StorageConnectionString, StorageContainerName);

            BlobClient blobClient = client.GetBlobClient(file.FileName);
            await blobClient.UploadAsync(Path.GetTempFileName());
            BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
            return blobProperties;

        }
    }
}