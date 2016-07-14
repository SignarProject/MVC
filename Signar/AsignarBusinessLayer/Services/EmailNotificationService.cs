using AsignarBusinessLayer.AsignarDatabaseDTOs;
using Microsoft.Azure;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AsignarBusinessLayer.Services
{
    public class EmailNotificationService
    {
        private readonly string _sendGridAPI;

        private readonly string _resetPasswordTemplateID;
        private readonly string _BugConditionChangedTemplateID;
        private readonly string _bugReassignedTemplateID;
                
        private const string _asignarBTSEmail = "asignarBTS@outlook.com";
        private const string _asignarBTSEmailPassword = "Password: 1qaz2wsX34";

        public EmailNotificationService()
        {
            _sendGridAPI = CloudConfigurationManager.GetSetting("AsignarNotificationAPI");

            _resetPasswordTemplateID = CloudConfigurationManager.GetSetting("ResetPasswordSendGridTemplateId");
            _BugConditionChangedTemplateID = CloudConfigurationManager.GetSetting("BugConditionChangedSendGridTemplateId");
            _bugReassignedTemplateID = CloudConfigurationManager.GetSetting("BugReassignedSendGridTemplateId");            
        }

        public void ResetPassword(UserDTO user)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(user.Email);
            myMessage.From = new MailAddress(_asignarBTSEmail, "Asignar-BTS Automatic Notification System");

            myMessage.EnableTemplateEngine("729623f4-ace6-4555-8f68-2c353b1e74c4");
            myMessage.AddSubstitution("%name%", new List<string> { user.Name });
            myMessage.AddSubstitution("%actionurl%", new List<string> { "SomeActionURL" });
            myMessage.Subject = "Reset Asignar-BTS account password";

            var transportWeb = new Web("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");
            transportWeb.DeliverAsync(myMessage).Wait();
        }
    }
}
