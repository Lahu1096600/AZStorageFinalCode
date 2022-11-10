using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageCURD.Models;
using AzureLabStorageProject.Repository;
using System.Threading.Tasks;
using System;
using AzureLabStorageProject.Models;

namespace AzureLabStorageProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureTableController : ControllerBase
    {
        public readonly ITableService _tableService;
        public AzureTableController(ITableService tableService)
        {
            _tableService = tableService;
        }





        [HttpGet]
        [Route("GetEntityAsync")]
        public async Task<IActionResult> GetEntityAsync([FromQuery] string category, string id)
        {
            return Ok(await _tableService.GetEntityAsync(category, id));
        }

        [HttpPost]
        [Route("PostAsync")]
        public async Task<IActionResult> PostAsync(GroceryItemEntity entity)
        {
            entity.PartitionKey = entity.Category;

            string Id = Guid.NewGuid().ToString();
            entity.Id = Id;
            entity.RowKey = Id;
            //entity.ETag = ETag.All;

            var createdEntity = await _tableService.UpsertEntityAsync(entity);
            return Ok(createdEntity);
        }


        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string category, string id)
        {
            await _tableService.DeleteEntityAsync(category, id);
            return NoContent();
        }

        [HttpDelete]
        [Route("dropTable")]
        public async Task<IActionResult> DropAzureStorageTable(string TableName)
        {
            await _tableService.DropAzureStorageTable(TableName);
            return NoContent();
        }
    }
}
