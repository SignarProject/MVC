using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AsignarWebJob.SerializationSignatures;
using Microsoft.WindowsAzure;

namespace AsignarWebJob.WebJobServices
{
    public class EmailNotificationService
    {
        private readonly string _sendGridAPI;

        private readonly string _userRegistrationTemplateID;
        private readonly string _resetPasswordTemplateID;
        private readonly string _BugConditionChangedTemplateID;
        private readonly string _bugReassignedTemplateID;
                
        private const string _asignarBTSEmail = "asignarBTS@outlook.com";
        private const string _asignarBTSEmailPassword = "Password: 1qaz2wsX34";

        public EmailNotificationService()
        {
            _sendGridAPI = CloudConfigurationManager.GetSetting("AsignarNotificationAPI");

            _userRegistrationTemplateID = CloudConfigurationManager.GetSetting("UserRegistrationSendGridTemplateId");
            _resetPasswordTemplateID = CloudConfigurationManager.GetSetting("ResetPasswordSendGridTemplateId");
            _BugConditionChangedTemplateID = CloudConfigurationManager.GetSetting("BugConditionChangedSendGridTemplateId");
            _bugReassignedTemplateID = CloudConfigurationManager.GetSetting("BugReassignedSendGridTemplateId");
        }

        public void UserRegistrartion(NotificationItem user)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(user.Email);
            myMessage.From = new MailAddress(_asignarBTSEmail, "Asignar-BTS Automatic Notification System");

            myMessage.EnableTemplateEngine(_userRegistrationTemplateID);
            myMessage.Subject = "Registration in Asignar-BTS";
            myMessage.Text = "Test";
            myMessage.Html = "Test";
            /*myMessage.AddSubstitution("%name%", new List<string> { user.Name });
            myMessage.AddSubstitution("%login%", new List<string> { user.Login });
            myMessage.AddSubstitution("%password%", new List<string> { user.Password });*/

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(myMessage).Wait();
        }

        public void ResetPassword(NotificationItem user)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(user.Email);
            myMessage.From = new MailAddress(_asignarBTSEmail, "Asignar-BTS Automatic Notification System");

            myMessage.EnableTemplateEngine(_resetPasswordTemplateID);
            myMessage.Subject = "Reset Asignar-BTS account password";
            myMessage.AddSubstitution("%name%", new List<string> { user.Name });

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(myMessage).Wait();
        }
    }
}
