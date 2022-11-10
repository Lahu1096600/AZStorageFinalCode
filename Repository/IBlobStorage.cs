using Azure.Storage.Blobs.Models;
using AzureLabStorageProject.Models;

namespace AzureLabStorageProject.Repository
{
    public interface IBlobStorage
    {
        Task<BlobProperties> UploadAsync(IFormFile file);


        Task<BlobDto> DownloadAsync(string blobFilename);


        Task<BlobResponseDto> DeleteAsync(string blobFilename);


        Task<List<BlobDto>> ListAsync();

    }
}
