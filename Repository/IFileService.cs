using Microsoft.WindowsAzure.Storage.File;

namespace AzureLabStorageProject.Repository
{
    public interface IFileService
    {
        Task<bool> CreateShareAsync(string shareName);
        Task<bool> CreateFileinRootDirectoryAsync(string shareName);
        Task<bool> UploadFile();
        FileResultSegment GetAllFilesFromDirectory(string fileShareName, string directoryName);
        Task<Boolean> DeleteAllAsync(string shareName, string directoryName);
    }
}
