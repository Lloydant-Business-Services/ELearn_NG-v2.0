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


    public class SendEmailDTO
    {
        public string VerificationGuid { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string Subject { get; set; }
        public string CTAUrl { get; set; }
        public string SenderEmail { get; set; } = "noreply@kulpayng.com";
        public string SenderName { get; set; } = "Kulpay";
        public EmailNotificationCategory EmailCategory { get; set; }
        public string Body { get; set; }
        public string InstitutionName { get; set; }
        public string AccessCode { get; set; }
        public string ButtonText { get; set; } = "Click To Verify";
        public long Id { get; set; }
        public string Year { get; set; } = DateTime.Now.Year.ToString();
        public string CollectionKey { get; set; }
        public string CollectionName { get; set; }
        public string Amount { get; set; }
        public string FixedAmount { get; set; }
        public string RoleName { get; set; }
    }


}
