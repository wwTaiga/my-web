using System.Collections.Generic;
using System.Collections.ObjectModel;
using MimeKit;

namespace MyWeb.Models
{
    public class EmailMessage
    {
        public Collection<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public EmailMessage(IEnumerable<string> to, string subject, string content)
        {
            To = new();
            foreach (string emailAddress in to)
            {
                To.Add(MailboxAddress.Parse(emailAddress));
            }
            Subject = subject;
            Content = content;
        }

    }

}
