﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace RentApp.Services
{
    public class SmtpService : ISmtpService
    {
        public void SendMail(string subject, string body, string emailTo)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress("titovrentavehicle@gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("titovrentavehicle@gmail.com", "drugtito");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
    }
}