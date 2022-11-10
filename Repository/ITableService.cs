using Azure.Data.Tables;
using AzureLabStorageProject.Models;


namespace AzureLabStorageProject.Repository
{
    public interface ITableService
    {
       Task<GroceryItemEntity> GetEntityAsync(string category, string Id);
        Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity groceryItemEntity);
        Task<bool> DeleteEntityAsync(string category, string Id);
        Task<bool> DropAzureStorageTable(string TableName);
    }
}
