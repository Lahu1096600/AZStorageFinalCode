using Azure.Storage.Queues.Models;
using Azure;

namespace AzureLabStorageProject.Repository
{
    public interface IQueueService
    {
        Task<bool> CreateQueueClient(string queueName);
        Task<bool> CreateQueue(string queueName);
        Response<SendReceipt> InsertMessage(string queueName, string message);
        Task<Response<PeekedMessage>> PeekMessage(string queueName);
        Task<UpdateReceipt> UpdateMessage(string queueName);
        Task<Boolean> DequeueMessage(string queueName);
        Task<bool> QueueAsync(string queueName);
        Task<int> GetQueueLength(string queueName);
        Task<Boolean> DeleteQueue(string queueName);

    }
}
