using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GemBox.Email;
using GemBox.Email.Pop;
using ScruffCalendar.Models;


namespace ScruffCalendar
{
    public class gmailScraper
    {
        string name { get; }
        string pass { get; }
        StringBuilder emails = new StringBuilder();
        private readonly Regex ZoomId = new Regex(@"(\d{3}\s\d{3,4}\s\d{3,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex ZoomJoinUrl = new Regex(@"https://SonomaState.zoom.us/j/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        List<string> ID = new List<String>();
        List<Meeting> meetings = new List<Meeting>();
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
                        foreach (Match match in ZoomId.Matches(body))
                        {
                            ID.Add(match.Value);
                        }
                        if (body.Contains("is inviting you to a scheduled Zoom meeting."))
                        {

                            MatchCollection matches = ZoomId.Matches(body);
                            MatchCollection joinlinks = ZoomJoinUrl.Matches(body);
                            long lid = 0;
                            string link = "";
                            if (matches.Count == 0 || joinlinks.Count == 0)
                            {
                                pops++;
                                continue;
                            }
                            if (matches.Count > 0)
                            {
                                Match match = ZoomId.Matches(body)[0];
                                string id = match.Value;
                                id.Replace(" ", "");
                                lid = long.Parse(id);
                            }    
                            if(joinlinks.Count >0)
                            {
                                Match linksys = ZoomJoinUrl.Matches(body)[0];
                                link = linksys.Value;
                            }
                           
                            var meeting = new Meeting()
                            {
                                 Id = lid,
                                JoinUrl = link,

                            };
                            meetings.Add(meeting);
                            emails.Append(body);
                           
                        }
                    pops++;
                }
            }
        }
        string getText()
        {
            return emails.ToString();
        }
     
        List<string> getLink()
        {
            return ID;


        }
        List<Meeting> GetMeetings()
        {
            return meetings;
        }
    
    }
}
 