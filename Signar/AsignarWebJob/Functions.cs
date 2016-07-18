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
        public static void EmailNotificationForUser([QueueTrigger("emailnotificationqueue")] NotificationUserItem message, TextWriter log)
        {
            var emailService = new EmailNotificationService();

            switch (message.LetterTemplate)
            {
                case NotificationUserItem.NotificationType.Registered:
                    {
                        emailService.UserRegistrartion(message);
                        break;
                    }
                case NotificationUserItem.NotificationType.ResetPassword:
                    {
                        emailService.ResetPassword(message);
                        break;
                    }
            }

            log.WriteLine("EmailNotificationSuccess!");
        }

        public static void EmailNotificationAboutBug([QueueTrigger("emailnotificationqueue")] NotificationBugItem message, TextWriter log)
        {
            var emailService = new EmailNotificationService();

            switch (message.LetterTemplate)
            {
                case NotificationUserItem.NotificationType.BugConditionChanged:
                    {
                        emailService.BugConditionChanged(message);
                        break;
                    }
                case NotificationUserItem.NotificationType.BugReassigned:
                    {
                        emailService.BugReassigneed(message);
                        break;
                    }
            }

            log.WriteLine("EmailNotificationSuccess!");
        }
    }
}
