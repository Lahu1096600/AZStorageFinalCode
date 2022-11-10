using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using AzureLabStorageProject.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;

namespace AzureLabStorageProject.Models
{
    public class FileShareService : IFileService
    {
        public IConfiguration _configuration;
        public string StorageAccountName = string.Empty;
        public string StorageAccountKey = string.Empty;
        public string StorageConnectionString = string.Empty;
        public FileShareService(IConfiguration configuration)
        {
            _configuration = configuration;

            StorageAccountKey = _configuration.GetConnectionString("StorageAccountKey");
            StorageAccountName = _configuration.GetConnectionString("StorageAccountName");
            StorageConnectionString = _configuration.GetConnectionString("StorageConnectionString");
        }
        /// <summary>
        /// CreateFileinRootDirectoryAsync
        /// </summary>
        /// <param name="shareName"></param>
        /// <returns></returns>
        public async Task<bool> CreateFileinRootDirectoryAsync(string shareName)
        {
            var azureStorageAccount = new CloudStorageAccount(new StorageCredentials(StorageAccountName, StorageAccountKey), true);


            var fileShare = azureStorageAccount.CreateCloudFileClient().GetShareReference(shareName);
           await fileShare.CreateIfNotExistsAsync();

            var rootDir = fileShare.GetRootDirectoryReference();
            await rootDir.GetFileReference("myfirstfile1110").UploadTextAsync("Test");

            return true;
        }
        /// <summary>
        /// CreateShareAsync
        /// </summary>
        /// <param name="shareName"></param>
        /// <returns></returns>
        public async Task<bool> CreateShareAsync(string shareName)
        {
            var azureStorageAccount = new CloudStorageAccount(new StorageCredentials(StorageAccountName, StorageAccountKey), true);

            var fileShare = azureStorageAccount.CreateCloudFileClient().GetShareReference("testfileshare");
            return await fileShare.CreateIfNotExistsAsync();
        }
        /// <summary>
        /// DeleteAllAsync
        /// </summary>
        /// <param name="shareName"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public async Task<Boolean> DeleteAllAsync(string shareName, string directoryName)
        {
            ShareClient shareClient = new ShareClient(StorageConnectionString, shareName);
            ShareDirectoryClient dirClient = shareClient.GetDirectoryClient(directoryName);
            Pageable<ShareFileItem> shareFileItems = dirClient.GetFilesAndDirectories();

            foreach (ShareFileItem item in shareFileItems)
            {
                if (item.IsDirectory)
                {
                    var subDir = dirClient.GetSubdirectoryClient(item.Name);
                    await subDir.DeleteAsync();
                }
                else
                {
                    await dirClient.DeleteFileAsync(item.Name);
                }
            }

            await dirClient.DeleteAsync();

            return true;
        }

        /// <summary>
        /// GetAllFilesFromDirectory
        /// </summary>
        /// <param name="fileShareName"></param>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        public FileResultSegment GetAllFilesFromDirectory(string fileShareName, string directoryName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));
            CloudFileClient cloudFileClient = cloudStorageAccount.CreateCloudFileClient();
            CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(fileShareName);
            CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
            CloudFileDirectory fileDirectory = rootDirectory.GetDirectoryReference(directoryName);

            List<IListFileItem> results = new List<IListFileItem>();
            FileContinuationToken token = null;
            FileResultSegment resultSegment = null;
            do
            {
                resultSegment = fileDirectory.ListFilesAndDirectoriesSegmentedAsync(token).GetAwaiter().GetResult();
                token = resultSegment.ContinuationToken;
            }
            while (token != null);
            return resultSegment;
        }

        /// <summary>
        /// UploadFile
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UploadFile()
        {
            string connectionString = _configuration.GetConnectionString("StorageConnectionString");

            var azureStorageAccount=  new CloudStorageAccount(new StorageCredentials(StorageAccountName,StorageAccountKey), true);

            var fileShareName = _configuration.GetConnectionString("fileShareName");



            var folderName = "directory1";
            var fileName = "Testfile.txt";
            var localFilePath = @"C:/Users/vmadmin/Downloads/testfile.txt";



            ShareClient share = new ShareClient(connectionString, fileShareName);



            var directory = share.GetDirectoryClient(folderName);



            var file = directory.GetFileClient(fileName);
            using FileStream stream = File.OpenRead(localFilePath);
            file.Create(stream.Length);
            file.UploadRange(
                new HttpRange(0, stream.Length),
                stream);
            return true;
        }
    }
}