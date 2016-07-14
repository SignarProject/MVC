using AsignarBusinessLayer.AsignarDatabaseDTOs;
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
        private const string _sendGridAPI = "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";

        private const string _resetPasswordTemplateID = "729623f4-ace6-4555-8f68-2c353b1e74c4";
        private const string _BugConditionChangedTemplateID = "5a6e24f6-7dd8-459c-9119-523d7ac98359";
        private const string _bugReassignedTemplateID = "985baa27-3be9-40ad-8339-aa0124d8141e";
                
        private const string _asignarBTSEmail = "asignarBTS@outlook.com";
        private const string _asignarBTSEmailPassword = "Password: 1qaz2wsX34";

        public async void ResetPassword(UserDTO user)
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
