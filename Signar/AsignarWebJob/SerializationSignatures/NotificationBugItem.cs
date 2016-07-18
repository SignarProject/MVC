using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsignarWebJob.SerializationSignatures
{
    public class NotificationBugItem
    {
        public NotificationUserItem.NotificationType LetterTemplate { get; set; }

        public string FullPrefix { get; set; }

        public string ProjectName { get; set; }

        public string AssigneeName { get; set; }

        public string AssigneeEmail { get; set; }

        public List<string> ActionUrlsList { get; set; }

        public NotificationBugItem()
        {
            ActionUrlsList = new List<string>();
        }
    }
}
