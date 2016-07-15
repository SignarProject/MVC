using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsignarDataAccessLayer.AzureASModel;
using AsignarBusinessLayer.AsignarDatabaseDTOs;
using AsignarDataAccessLayer.SerializationSignatures;
using Newtonsoft.Json;

namespace AsignarBusinessLayer.Services
{
    public class NotificationQueueService
    {
        public AsignarQueueModel QueueModel { get; set; }

        public NotificationQueueService()
        {
            QueueModel = new AsignarQueueModel("emailnotificationqueue", false);
        }

        public void UserRegistration(UserDTO item, List<string> actionUrlsList)
        {
            NotificationItem notificationItem = CreateNotificationItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationItem.NotificationType.Registered;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        public void ResetUserPassword(UserDTO item, List<string> actionUrlsList)
        {
            NotificationItem notificationItem = CreateNotificationItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationItem.NotificationType.ResetPassword;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        private NotificationItem CreateNotificationItem(UserDTO item, List<string> actionUrlsList)
        {
            var notificationItem = new NotificationItem();
                        
            notificationItem.Name = item.Name;
            notificationItem.Surname = item.Surname;
            notificationItem.Email = item.Email;
            notificationItem.Login = item.Login;
            notificationItem.Password = item.Password;
            notificationItem.ActionUrlsList = actionUrlsList;

            return notificationItem;
        }
    }
}
