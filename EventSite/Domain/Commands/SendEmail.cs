using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using EventSite.Domain.Infrastructure;
using RazorEngine;

namespace EventSite.Domain.Commands {
    public abstract class SendEmail : Work {
        static readonly ConcurrentBag<string> CompiledTemplates = new ConcurrentBag<string>();
        static readonly object CompiledTemplatesLock = new object();

        public string ToName { get; set; }
        public string ToEmail { get; set; }

        public string Subject { get; set; }
        public string Template { get; set; }

        public override void Process() {
            try {
                var message = new MailMessage();

                message.To.Add(new MailAddress(ToEmail, ToName));
                message.From = new MailAddress(State.Settings.FromEmail, State.Settings.FromEmailName);
                message.Subject = Subject;
                message.Body = Razor.Run(ensureTemplate(Template), this);

                if(State.RunningInProduction()) {
                    var smtpClient = new SmtpClient(State.Settings.SmtpHost, State.Settings.SmtpPort);
                    var credentials = new System.Net.NetworkCredential(State.Settings.SmtpUsername, State.Settings.SmtpPassword);

                    smtpClient.Credentials = credentials;
                    smtpClient.Send(message);
                } else {
                    Log.Info(message.Body);
                }
            }
            catch (Exception ex) {
                Log.Error(ex); //not the best practice, but we don't care too much if this fails and we don't want to break anything else
            }
        }

        string ensureTemplate(string templateName) {
            if(!CompiledTemplates.Contains(templateName)) {
                lock(CompiledTemplatesLock) {
                    if(!CompiledTemplates.Contains(templateName)) {
                        var assembly = Assembly.GetExecutingAssembly();
                        var resourceName = "EventSite.EmailTemplates." + templateName + ".template";

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