using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

 namespace BusinessLayer.Services.Email

{
    public class EmailService : IEmailService
    {
        string _template = "LiteHR.Services.Email.default.cshtml";
        string _templateAlt = "APIs.EmailTemplates.default.html";
        IWebHostEnvironment HostingEnvironment { get; }


        private readonly IFluentEmail _email;
        private readonly IConfiguration _configuration;
        private readonly string frontEndUrl;

        public EmailService([FromServices] IFluentEmail email, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _email = email;

            _configuration = configuration;
            //var sender = new MailgunSender(_configuration.GetValue<string>("MailGun:domain"),
            //_configuration.GetValue<string>("MailGun:apiKey"));
            var sender = new MailgunSender("nrf.lloydant.com", "key-8540f3ef6a66cdaf8d9121f11c99aa6b");
            _email.Sender = sender;
            frontEndUrl = _configuration.GetValue<string>("Url:frontUrl");
            HostingEnvironment = hostingEnvironment;


        }

        public async Task SendMail(EmailDto sendEmailDto, string template)
        {
            try
            {
                string pathToEmailFile = $"{HostingEnvironment.WebRootPath}/EmailTemplates/LoginAlert.html";
                if (!string.IsNullOrEmpty(template))
                {
                    _template = template;
                }
                var templateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "default.html");

                //var parsedTemplate = ParseTemplate(_template);
                var parsedTemplate = templateFilePath;
                if (!string.IsNullOrEmpty(parsedTemplate))
                {
                    try
                    {
                        var sendStatus = await _email
                       .SetFrom(sendEmailDto.SenderEmail, sendEmailDto.SenderName)
                       .To(sendEmailDto.ReceiverEmail)
                       .Subject(sendEmailDto.Subject)
                       .UsingTemplate(parsedTemplate, sendEmailDto)
                       .SendAsync();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
           

        }

        private string ParseTemplate(string path)
        {
            string result;
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
        public async Task EmailFormatter(EmailDto sendEmailDto)
        {
          
                    sendEmailDto.Body = string.Format("Hi " + sendEmailDto.ReceiverName + "!" + " You requested a password request. Click the Reset Password Button below to continue and confirm that the request came from you.");
                    sendEmailDto.Subject = "Job Notification";
                    //sendEmailDto.CTAUrl = localUrl + "VerifyEmail?guid=" + sendEmailDto.VerificationGuid;
                    sendEmailDto.CTAUrl = frontEndUrl + "VerifyEmail?guid=" + sendEmailDto.VerificationGuid + "&email=" + sendEmailDto.ReceiverEmail;
                    await SendMail(sendEmailDto, _templateAlt);
                   
            }
        
    }
}
