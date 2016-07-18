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
            NotificationUserItem notificationItem = CreateNotificationUserItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationUserItem.NotificationType.Registered;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        public void ResetUserPassword(UserDTO item, List<string> actionUrlsList)
        {
            NotificationUserItem notificationItem = CreateNotificationUserItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationUserItem.NotificationType.ResetPassword;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        public void BugConditionChanged(BugDTO item, List<string> actionUrlsList)
        {
            NotificationBugItem notificationItem = CreateNotificationBugItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationUserItem.NotificationType.BugConditionChanged;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        public void BugReassigned(BugDTO item, List<string> actionUrlsList)
        {
            NotificationBugItem notificationItem = CreateNotificationBugItem(item, actionUrlsList);
            notificationItem.LetterTemplate = NotificationUserItem.NotificationType.BugReassigned;

            string jsonString = JsonConvert.SerializeObject(notificationItem);

            QueueModel.EnqueueMessage(jsonString);
        }

        private NotificationUserItem CreateNotificationUserItem(UserDTO item, List<string> actionUrlsList)
        {
            var notificationItem = new NotificationUserItem();
                        
            notificationItem.Name = item.Name;
            notificationItem.Surname = item.Surname;
            notificationItem.Email = item.Email;
            notificationItem.Login = item.Login;
            notificationItem.Password = item.Password;
            notificationItem.ActionUrlsList = actionUrlsList;

            return notificationItem;
        }

        private NotificationBugItem CreateNotificationBugItem(BugDTO item, List<string> actionUrlsList)
        {
            var notificationItem = new NotificationBugItem();

            notificationItem.FullPrefix = item.Prefix;
            notificationItem.ProjectName = item.Project.Name;
            notificationItem.AssigneeName = item.User.Name;
            notificationItem.AssigneeEmail = item.User.Email;
            notificationItem.ActionUrlsList = actionUrlsList;

            return notificationItem;
        }
    }
}
