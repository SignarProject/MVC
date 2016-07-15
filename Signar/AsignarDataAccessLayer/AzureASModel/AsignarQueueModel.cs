using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AsignarDataAccessLayer.AzureASModel
{
    public class AsignarQueueModel
    {
        public CloudQueue Queue { get; set; }

        public AsignarQueueModel(string queueName, bool isNewQueue)
        {
            string appSetting = CloudConfigurationManager.GetSetting("AsignarAzureStorage");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSetting);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            Queue = queueClient.GetQueueReference(queueName);
            if(isNewQueue)
            {
                Queue.CreateIfNotExists();
            }            
        }

        public void EnqueueMessage(string message)
        {
            var queueMessage = new CloudQueueMessage(message);
            Queue.AddMessage(queueMessage, new TimeSpan(1, 0, 0));
        }
    }
}
