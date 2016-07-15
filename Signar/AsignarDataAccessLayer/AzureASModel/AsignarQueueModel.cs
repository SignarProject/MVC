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

        public AsignarQueueModel()
        {
            string appSetting = CloudConfigurationManager.GetSetting("AsignarAzureStorage");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(appSetting);

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            Queue = queueClient.GetQueueReference("emailnotificationqueue");
            Queue.CreateIfNotExists();
        }

        public void Enqueue(string emailAdress)
        {
            var message = new CloudQueueMessage(emailAdress);
            Queue.AddMessage(message);
        }
    }
}
