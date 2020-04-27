using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GemBox.Email;
using GemBox.Email.Pop;


namespace ScruffCalendar
{
    public class gmailScraper
    {
        gmailScraper(string name, string pass)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");

            using (PopClient pop = new PopClient("pop.gmail.com"))
            {
                // Connect and login.
                pop.Connect();
                Console.WriteLine("Connected.");

                pop.Authenticate(name, pass);
                Console.WriteLine("Authenticated.");

                // Check if there are any messages available on the server.
                if (pop.GetCount() == 0)
                    return;

                // Download message with sequence number 1 (the first message).
                MailMessage message = pop.GetMessage(1);

                // Display message sender and subject.
                Console.WriteLine();
                Console.WriteLine($"From: {message.From}");
                Console.WriteLine($"Subject: {message.Subject}");

                // Display message body.
                Console.WriteLine("Body:");
                string body = string.IsNullOrEmpty(message.BodyHtml) ?
                    message.BodyText :
                    message.BodyHtml;
                Console.WriteLine(body);
            }

        }

    }
}
