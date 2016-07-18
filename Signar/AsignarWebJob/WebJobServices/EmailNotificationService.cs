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
        private readonly string _bugConditionChangedTemplateID;
        private readonly string _bugReassignedTemplateID;
                
        private const string _asignarBTSEmail = "asignarBTS@outlook.com";
        private const string _asignarNotificationName = "Asignar-BTS Automatic Notification System";
        private const string _asignarBTSEmailPassword = "Password: 1qaz2wsX34";

        public EmailNotificationService()
        {
            _sendGridAPI = CloudConfigurationManager.GetSetting("AsignarNotificationAPI");

            _userRegistrationTemplateID = CloudConfigurationManager.GetSetting("UserRegistrationSendGridTemplateId");
            _resetPasswordTemplateID = CloudConfigurationManager.GetSetting("ResetPasswordSendGridTemplateId");
            _bugConditionChangedTemplateID = CloudConfigurationManager.GetSetting("BugConditionChangedSendGridTemplateId");
            _bugReassignedTemplateID = CloudConfigurationManager.GetSetting("BugReassignedSendGridTemplateId");
        }

        public void UserRegistrartion(NotificationUserItem user)
        {
            var letter = new SendGridMessage();

            letter.AddTo(user.Email);
            letter.From = new MailAddress(_asignarBTSEmail, _asignarNotificationName);

            letter.EnableTemplateEngine(_userRegistrationTemplateID);

            letter.Subject = "Registration in Asignar-BTS";
            letter.AddSubstitution("%name%", new List<string> { string.Concat(user.Name, " ", user.Surname) });
            letter.AddSubstitution("%login%", new List<string> { user.Login });
            letter.AddSubstitution("%password%", new List<string> { user.Password });
            letter.Text = "Some text";
            letter.Html = "Some html";

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(letter).Wait();
        }

        public void ResetPassword(NotificationUserItem user)
        {
            var letter = new SendGridMessage();

            letter.AddTo(user.Email);
            letter.From = new MailAddress(_asignarBTSEmail, _asignarNotificationName);

            letter.EnableTemplateEngine(_resetPasswordTemplateID);

            letter.Subject = "Reset Asignar-BTS account password";
            letter.AddSubstitution("%name%", new List<string> { user.Name });
            letter.AddSubstitution("%new_password%", new List<string> { user.Password });
            letter.Text = "Some text";
            letter.Html = "Some html";

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(letter).Wait();
        }

        public void BugConditionChanged(NotificationBugItem bug)
        {
            var letter = new SendGridMessage();

            letter.AddTo(bug.AssigneeEmail);
            letter.From = new MailAddress(_asignarBTSEmail, _asignarNotificationName);

            letter.EnableTemplateEngine(_bugConditionChangedTemplateID);

            letter.Subject = "Your task condition changed in Asignar-BTS";
            letter.AddSubstitution("%name%", new List<string> { bug.AssigneeName });
            letter.AddSubstitution("%bug_prefix%", new List<string> { bug.FullPrefix });
            letter.AddSubstitution("%project_name%", new List<string> { bug.ProjectName });
            letter.Text = "Some text";
            letter.Html = "Some html";

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(letter).Wait();
        }

        public void BugReassigneed(NotificationBugItem bug)
        {
            var letter = new SendGridMessage();

            letter.AddTo(bug.AssigneeEmail);
            letter.From = new MailAddress(_asignarBTSEmail, _asignarNotificationName);

            letter.EnableTemplateEngine(_bugReassignedTemplateID);

            letter.Subject = "Task was reassigneed to you in Asignar-BTS";
            letter.AddSubstitution("%name%", new List<string> { bug.AssigneeName });
            letter.AddSubstitution("%bug_prefix%", new List<string> { bug.FullPrefix });
            letter.AddSubstitution("%project_name%", new List<string> { bug.ProjectName });
            letter.Text = "Some text";
            letter.Html = "Some html";

            var transportWeb = new Web(_sendGridAPI);
            transportWeb.DeliverAsync(letter).Wait();
        }
    }
}
