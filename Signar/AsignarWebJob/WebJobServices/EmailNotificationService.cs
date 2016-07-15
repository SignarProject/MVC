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
        private readonly string _sendGridAPI = "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";

        private readonly string _resetPasswordTemplateID = "729623f4-ace6-4555-8f68-2c353b1e74c4";
        private readonly string _BugConditionChangedTemplateID = "5a6e24f6-7dd8-459c-9119-523d7ac98359";
        private readonly string _bugReassignedTemplateID = "985baa27-3be9-40ad-8339-aa0124d8141e";
                
        private const string _asignarBTSEmail = "asignarBTS@outlook.com";
        private const string _asignarBTSEmailPassword = "Password: 1qaz2wsX34";

        public EmailNotificationService()
        {
            _sendGridAPI = CloudConfigurationManager.GetSetting("AsignarNotificationAPI");
        }

        public async void ResetPassword(UserJSONSignature user)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(user.Email);
            myMessage.From = new MailAddress(_asignarBTSEmail, "Asignar-BTS Automatic Notification System");

            myMessage.EnableTemplateEngine(_resetPasswordTemplateID);
            myMessage.Html = "test";
            myMessage.Text = "Test";
            myMessage.Subject = "Reset Asignar-BTS account password";

            var transportWeb = new Web(_sendGridAPI);
            await transportWeb.DeliverAsync(myMessage);
        }
    }
}
