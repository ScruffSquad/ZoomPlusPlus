using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Email;
using GemBox.Email.Pop;


namespace ScruffCalendar
{
    public class gmailScraper
    {
        string name { get; }
        string pass { get; }
        StringBuilder emails = new StringBuilder(); 

        gmailScraper(string _name, string _pass)
        {
            // If using Professional version, put your serial key below.
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            name = _name;
            pass = _pass;


        }
        void connect()
        {
            using (PopClient pop = new PopClient("pop.gmail.com"))
            {
                int count = pop.GetCount();
                int pops = 1;
                // Connect and login.
                pop.Connect();
                Console.WriteLine("Connected.");

                pop.Authenticate(name, pass);
                Console.WriteLine("Authenticated.");

                // Check if there are any messages available on the server.
                if (pop.GetCount() == 0)
                    return;

                while(pops <= count)
                    {
                    // Download message with sequence number 1 (the first message).
                    MailMessage message = pop.GetMessage(pops);

                    // Display message sender and subject.
                    //Console.WriteLine();
                    //Console.WriteLine($"From: {message.From}");
                    //Console.WriteLine($"Subject: {message.Subject}");
                     emails.Append($"From: {message.From}");
                     emails.Append($"Subject: {message.Subject}");

                    // Display message body.
                    // Console.WriteLine("Body:");
                     string body = string.IsNullOrEmpty(message.BodyHtml) ?
                         message.BodyText :
                          message.BodyHtml;
                        //Console.WriteLine(body);
                        emails.Append(body);
                       pops++;                
                    }
                }
        }
        string getText()
        {
            return emails.ToString();
        }
    }
}
 