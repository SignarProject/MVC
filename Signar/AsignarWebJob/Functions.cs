using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using AsignarWebJob.SerializationSignatures;
using AsignarWebJob.WebJobServices;

namespace AsignarWebJob
{
    public class Functions
    {
        public static void EmailNotificationQueueMessage([QueueTrigger("emailnotificationqueue")] NotificationItem message, TextWriter log)
        {
            var emailService = new EmailNotificationService();

            switch (message.LetterTemplate)
            {
                case NotificationItem.NotificationType.Registered:
                    {
                        emailService.UserRegistrartion(message);
                        break;
                    }
                case NotificationItem.NotificationType.ResetPassword:
                    {
                        emailService.ResetPassword(message);
                        break;
                    }
            }

            log.WriteLine("EmailNotificationSuccess!");
        }
    }
}
