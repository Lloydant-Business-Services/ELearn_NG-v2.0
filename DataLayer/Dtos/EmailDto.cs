using DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class EmailDto
    {
        public string VerificationGuid { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string Subject { get; set; }
        public string SenderEmail { get; set; } = "support@elearnng.com";
        public string SenderName { get; set; } = "ElearnNG";
        public string message { get; set; }
        public string OTP { get; set; }
        public string ButtonText { get; set; }
        public string MailGunTemplate { get; set; }
        public EmailNotificationCategory NotificationCategory { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
    }
}
