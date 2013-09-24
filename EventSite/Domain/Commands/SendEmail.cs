using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using RazorEngine;

namespace EventSite.Domain.Commands {
    public abstract class SendEmail {
        public static ConcurrentBag<string> CompiledTemplates = new ConcurrentBag<string>();
        public static object CompiledTemplatesLock = new object();

        public string ToName { get; set; }
        public string ToEmail { get; set; }

        public string Subject { get; set; }
        public string Template { get; set; }

        public void Execute() {
            try {
                var message = new MailMessage();

                message.To.Add(new MailAddress(ToEmail, ToName));
                message.From = new MailAddress("from@example.com", "From Name");
                message.Subject = Subject;
                message.Body = Razor.Run(ensureTemplate(Template), this);
                
                var smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                var credentials = new System.Net.NetworkCredential("username@domain.com", "yourpassword");
                smtpClient.Credentials = credentials;

                smtpClient.Send(message);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        string ensureTemplate(string templateName) {
            if(!CompiledTemplates.Contains(templateName)) {
                lock(CompiledTemplatesLock) {
                    if(!CompiledTemplates.Contains(templateName)) {
                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = "EventSite.EmailTemplates." + templateName + ".cshtml";

                        using(var stream = assembly.GetManifestResourceStream(resourceName)) {
                            if(stream == null) {
                                throw new Exception("Failed to locate email template '" + templateName + "'.");
                            }

                            var templateText = new StreamReader(stream).ReadToEnd();
                            Razor.Compile(templateText, GetType(), templateName);
                            CompiledTemplates.Add(templateName);
                        }
                    }
                }
            }

            return templateName;
        }
    }
}