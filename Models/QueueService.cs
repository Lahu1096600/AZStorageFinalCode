using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureLabStorageProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AzureLabStorageProject.Models
{
    public class QueueService : IQueueService
    {
        public IConfiguration _configuration;
        public string connectionString = string.Empty;
        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("StorageConnectionString");
        }

        /// <summary>
        /// CreateQueue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task< bool> CreateQueue(string queueName)
        {
            try
            {
                QueueClient queueClient = new QueueClient(connectionString, queueName);
                queueClient.CreateIfNotExists();
                if (queueClient.Exists())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// CreateQueueClient
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<bool> CreateQueueClient(string queueName)
        {
         
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            return true;
        }


        /// <summary>
        /// DequeueMessage
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<Boolean> DequeueMessage(string queueName)
        {
            
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                QueueMessage[] message =  queueClient.ReceiveMessages();

                queueClient.DeleteMessage(message[0].MessageId, message[0].PopReceipt);
                return true;
            }
            return false;
        }

        /// <summary>
        /// GetQueueLength
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<int> GetQueueLength(string queueName)
        {
           
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            int messageCount = 0;
            if (queueClient.Exists())
            {
                object queuClient = null;
                QueueProperties properties = await queueClient.GetPropertiesAsync();
                messageCount = properties.ApproximateMessagesCount;

            }

            return messageCount;
        }

        /// <summary>
        /// InsertMessage
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Response<SendReceipt> InsertMessage(string queueName, string message)
        {
            Response<SendReceipt> result = null;

           
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            if (queueClient.Exists())
            {
                result = queueClient.SendMessage(message);
            }
            return result;
        }

        /// <summary>
        /// PeekMessage
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<Response<PeekedMessage>> PeekMessage(string queueName)
        {
           
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            Response<PeekedMessage> peekMessage = null;

            if (queueClient.Exists())
            {
                peekMessage = await queueClient.PeekMessageAsync();


            }
            return peekMessage;
        }

        /// <summary>
        /// QueueAsync
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<bool> QueueAsync(string queueName)
        {
           // string connectionString = _configuration.GetConnectionString("StorageConnectionString");

            QueueClient queueClient = new QueueClient(connectionString, queueName);
           await queueClient.CreateIfNotExistsAsync();
            if (await queueClient.ExistsAsync())
            {
                Console.WriteLine($"Queue '{queueClient.Name}' created");
                // add message to queue
                await queueClient.SendMessageAsync("Hello world");

                // get message from queue

                QueueMessage[] retrivedMessage = queueClient.ReceiveMessages();


                // get message from queue
                await queueClient.DeleteMessageAsync(retrivedMessage[0].MessageId, retrivedMessage[0].PopReceipt);

                // delete  queue

                await queueClient.DeleteAsync();
                return true;

            }
            else
            {
                Console.WriteLine($"Queue '{queueClient.Name}' already created.");
               
            }

            return false;
        }

        /// <summary>
        /// UpdateMessage
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<UpdateReceipt> UpdateMessage(string queueName)
        {
            
            QueueClient queueClient = new QueueClient(connectionString, queueName);
            UpdateReceipt updateReceipt = null;
            if (queueClient.Exists())
            {
                QueueMessage[] message = queueClient.ReceiveMessages();

                updateReceipt = await queueClient.UpdateMessageAsync(message[0].MessageId, message[0].PopReceipt, "Updated Message", TimeSpan.FromSeconds(1));

            }
            return updateReceipt;
        }

        /// <summary>
        /// DeleteQueue
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteQueue(string queueName)
        {
            
            QueueClient queueClient = new QueueClient(connectionString, queueName);

            if (queueClient.Exists())
            {
                queueClient.DeleteAsync();

                return true;
            }
            return false;
        }
    }
}