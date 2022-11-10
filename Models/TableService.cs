using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using AzureLabStorageProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace AzureLabStorageProject.Models
{
    public class TableService : ITableService
    {
        private const string TableName = "Item";
        public IConfiguration _configuration;
        public TableService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// GetTableClient
        /// </summary>
        /// <returns></returns>
        private async Task<TableClient> GetTableClient()
            {
                var serviceClient = new TableServiceClient(_configuration.GetConnectionString("StorageConnectionString"));
                var tableClient = serviceClient.GetTableClient(TableName);
                await tableClient.CreateIfNotExistsAsync();
                return tableClient;
            }


        /// <summary>
        /// GetEntityAsync
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GroceryItemEntity> GetEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<GroceryItemEntity>(category, id);
        }


        /// <summary>
        /// UpsertEntityAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<GroceryItemEntity> UpsertEntityAsync(GroceryItemEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }


        /// <summary>
        /// DeleteEntityAsync
        /// </summary>
        /// <param name="category"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(category, id);
            return true;
        }



        /// <summary>
        /// DropAzureStorageTable
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public async Task<bool> DropAzureStorageTable(string TableName)
        {
            CloudStorageAccount cloudStorageAccount =   CloudStorageAccount.Parse(_configuration.GetConnectionString("StorageConnectionString"));
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();
           
            CloudTable cloudTable = tableClient.GetTableReference(TableName);
            if (cloudTable!=null)
            {
               await cloudTable.DeleteIfExistsAsync();
                return true;
            }

            return false;
        }
    }
}