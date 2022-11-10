using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.File;
using AzureLabStorageProject.Models;
using AzureLabStorageProject.Repository;
using StorageCURD.Models;

namespace AzureLabStorageProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileShareController : ControllerBase
    {
        private readonly IFileService _storageService;
        public FileShareController(IFileService fileService)
        {
            _storageService = fileService;
        }
       

        [HttpPost]
        [Route("CreateShareAsync")]
        public Task<bool> CreateShareAsync(string shareName)
        {
            var result = _storageService.CreateShareAsync(shareName);
            return result;
        }

        [HttpPost]
        [Route("CreateFileinRootDirectoryAsync")]
        public Task<bool> CreateFileinRootDirectoryAsync([FromQuery] string shareName)
        {
            var result = _storageService.CreateFileinRootDirectoryAsync(shareName);
            return result;
        }

        [HttpPost]
        [Route("UploadFile")]
        public Task<bool> UploadFile()
        {

            var result = _storageService.UploadFile();
            return result;
        }

        [HttpGet]
        [Route("GetAllFilesFromDirectory")]
        public AZCustomeResponse GetAllFilesFromDirectory([FromQuery] string shareName, string directoryName)
        {
            AZCustomeResponse customeResponse = new AZCustomeResponse();
            //FileResultSegment fileResultSegment = _storageService.GetAllFilesFromDirectory(shareName, directoryName);
            customeResponse.Result = _storageService.GetAllFilesFromDirectory(shareName, directoryName);
            return customeResponse;

        }

        [HttpGet]
        [Route("DeleteAllFilesAsync")]
        public Task<Boolean> DeleteAllFilesAsync([FromQuery] string shareName, string directoryName)
        {

            var result = _storageService.DeleteAllAsync(shareName, directoryName);
            return result;
        }
    }
}
