using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using AsignarWebJob.SerializationSignatures;

namespace AsignarWebJob
{
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessQueueMessage([QueueTrigger("emailnotificationqueue")] UserJSONSignature message, TextWriter log)
        {
            log.WriteLine(message);
        }
    }
}
