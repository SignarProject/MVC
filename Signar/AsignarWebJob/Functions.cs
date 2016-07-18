using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using AsignarWebJob.SerializationSignatures;
using AsignarWebJob.WebJobServices;
using Newtonsoft.Json;

namespace AsignarWebJob
{
    public class Functions
    {
        public static void EmailNotification([QueueTrigger("emailnotificationqueue")] string message, TextWriter log)
        {
            var emailService = new EmailNotificationService();

            var deserializedUserMessage = JsonConvert.DeserializeObject<NotificationUserItem>(message);

            switch (deserializedUserMessage.LetterTemplate)
            {
                case NotificationUserItem.NotificationType.Registered:
                    {
                        emailService.UserRegistrartion(deserializedUserMessage);
                        break;
                    }
                case NotificationUserItem.NotificationType.ResetPassword:
                    {
                        emailService.ResetPassword(deserializedUserMessage);
                        break;
                    }
                case NotificationUserItem.NotificationType.BugConditionChanged:
                    {
                        var deserializedBugMessage = JsonConvert.DeserializeObject<NotificationBugItem>(message);
                        emailService.BugConditionChanged(deserializedBugMessage);
                        break;
                    }
                case NotificationUserItem.NotificationType.BugReassigned:
                    {
                        var deserializedBugMessage = JsonConvert.DeserializeObject<NotificationBugItem>(message);
                        emailService.BugReassigneed(deserializedBugMessage);
                        break;
                    }
            }

            log.WriteLine("EmailNotificationSuccess!");
        }
    }
}
