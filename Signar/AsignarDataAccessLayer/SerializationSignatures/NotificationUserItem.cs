using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarDataAccessLayer.SerializationSignatures
{
    public class NotificationUserItem
    {
        public enum NotificationType
        {
            Registered = 0,
            ResetPassword = 1,
            BugConditionChanged = 2,
            BugReassigned = 3
        }

        public NotificationType LetterTemplate { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public List<string> ActionUrlsList { get; set; }

        public NotificationUserItem()
        {
            ActionUrlsList = new List<string>();
        }
    }
}
