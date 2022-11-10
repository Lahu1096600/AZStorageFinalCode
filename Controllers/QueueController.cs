using Azure;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureLabStorageProject.Repository;

namespace AzureLabStorageProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueStorageService;

        public QueueController(IQueueService queueService)
        {
            _queueStorageService = queueService;
        }

      

        //[HttpGet]
        //[Route("CreateQueueClient")]
        //public Task<Boolean> CreateQueueClient([FromQuery] string queueName)
        //{
        //    var result = _queueStorageService.CreateQueueClient(queueName);
        //    return result;

        //}

        [HttpPost]
        [Route("CreateQueue")]
        public Task<bool> CreateQueue(string queueName)
        {
            var result = _queueStorageService.CreateQueue(queueName);
            return result;
        }

        [HttpPost]
        [Route("InsertMessage")]
        public Response<SendReceipt> InsertMessage(string queueName, string message)
        {
            var result = _queueStorageService.InsertMessage(queueName, message);
            return result;

        }

        [HttpPost]
        [Route("PeekMessage")]
        public async Task<Response<PeekedMessage>> PeekMessage(string queueName)
        {
            Response<PeekedMessage> peekMessage = null;
            peekMessage = await _queueStorageService.PeekMessage(queueName);
            return peekMessage;

        }

        [HttpPost]
        [Route("UpdateMessage")]
        public async Task<UpdateReceipt> UpdateMessage(string queueName)
        {
            var result = await _queueStorageService.UpdateMessage(queueName);
            return result;
        }

        [HttpPost]
        [Route("DequeueMessage")]
        public Task<Boolean> DequeueMessage(string queueName)
        {
            var result = _queueStorageService.DequeueMessage(queueName);
            return result;
        }

        

        //[HttpGet]
        //[Route("GetQueueLength")]
        //public async Task<int> GetQueueLength([FromQuery] string queueName)
        //{
        //    var result = await _queueStorageService.GetQueueLength(queueName);
        //    return result;
        //}

        [HttpDelete]
        [Route("DeleteQueue")]
        public Task<bool> DeleteQueue(string queueName)
        {
            var result = _queueStorageService.DeleteQueue(queueName);
            return result;
        }
    }
}
