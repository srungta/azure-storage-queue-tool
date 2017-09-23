using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Collections.Async;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azure_storage_queue_tool
{
    public class QueueHelper
    {
        private CloudQueue queue;
        private readonly CloudQueueClient queueClient;

        public QueueHelper(string connectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            queueClient = storageAccount.CreateCloudQueueClient();
        }

        public async Task SendMessageToQueueAsync<T>(string queueName, T messageToBeSent, bool createIfNotExist)
        {
            queue = queueClient.GetQueueReference(queueName);
            if (!queue.Exists() && !createIfNotExist)
            {
                throw new AggregateException("Queue does not exist.");
            }
            await queue.CreateIfNotExistsAsync();
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(messageToBeSent));
            await queue.AddMessageAsync(message);
        }

        public async Task SendBatchMessagesToQueueAsync<T>(string queueName, IEnumerable<T> messagesToBeSent, bool createIfNotExist)
        {
            queue = queueClient.GetQueueReference(queueName);
            if (!queue.Exists() && !createIfNotExist)
            {
                throw new AggregateException("Queue does not exist.");
            }
            await queue.CreateIfNotExistsAsync();
            await messagesToBeSent.ParallelForEachAsync(async messageToBeSent =>
             {
                 var message = new CloudQueueMessage(JsonConvert.SerializeObject(messageToBeSent));
                 await queue.AddMessageAsync(message);
             });
        }
    }
}
