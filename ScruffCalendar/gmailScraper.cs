using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using ScruffCalendar.Models;
using ScruffCalendar.Zoom;

namespace ScruffCalendar
{
    public class gmailScraper
    {
        string name { get; }
        string pass { get; }
        StringBuilder emails = new StringBuilder();
        private readonly Regex ZoomId = new Regex(@"(\d{3}\s\d{3,4}\s\d{3,4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex ZoomJoinUrl = new Regex(@"https://SonomaState.zoom.us/j/\d+(\?pwd=[\d\w]+)*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly Regex ZoomTime = new Regex(@"Time: (\w+ \d+, \d+ \d+:\d+ \w{2})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        List<string> ID = new List<String>();
        List<Meeting> meetings = new List<Meeting>();
        public gmailScraper(string _name, string _pass)
        {
            // If using Professional version, put your serial key below.
            //ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            name = _name;
            pass = _pass;


        }
        public void connect()
        {
            using (ImapClient client = new ImapClient())
            //  using (PopClient pop = new PopClient("pop.gmail.com"))
            {

                int pops = 1;
                // Connect and login.
                client.Connect("imap.gmail.com", 993, true);
                Console.WriteLine("Connected.");

                client.Authenticate(name, pass);
                Console.WriteLine("Authenticated.");

                var inbox = client.Inbox;
                inbox.Open(MailKit.FolderAccess.ReadOnly);
                int count = inbox.Count;

                // Check if there are any messages available on the server.
                if (inbox.Count == 0)
                    return;

                var query = SearchQuery.DeliveredAfter(DateTime.Parse("2020-03-11")).And(SearchQuery.BodyContains("is inviting you to a scheduled Zoom meeting."));

                foreach (var uid in inbox.Search(query))
                {
                    // Download message.
                    MimeMessage message = inbox.GetMessage(uid);

                    // Display message sender and subject.
                    //Console.WriteLine();
                    //Console.WriteLine($"From: {message.From}");
                    //Console.WriteLine($"Subject: {message.Subject}");
                    emails.Append($"From: {message.From}");
                    emails.Append($"Subject: {message.Subject}");

                    // Display message body.
                    // Console.WriteLine("Body:");
                    string body = string.IsNullOrEmpty(message.HtmlBody) ?
                        message.TextBody :
                         message.HtmlBody;


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
                            id = id.Replace(" ", "");
                            lid = long.Parse(id);
                        }
                        if (joinlinks.Count > 0)
                        {
                            Match linksys = ZoomJoinUrl.Matches(body)[0];
                            link = linksys.Value;
                        }

                        MatchCollection times = ZoomTime.Matches(body);
                        DateTime time = new DateTime();
                        if (times.Count > 0)
                        {
                            Match match = ZoomTime.Matches(body)[0];
                            time = DateTime.ParseExact(match.Groups[1].Value, "MMM d, yyyy hh:mm tt", CultureInfo.InvariantCulture);
                        }

                        var meeting = new Meeting()
                        {
                            Id = lid,
                            JoinUrl = link,
                            StartTime = time,
                        };

                        Console.WriteLine(lid);

                        meetings.Add(meeting);
                        emails.Append(body);

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
        public List<Meeting> GetMeetings()
        {
            return meetings;
        }
    
    }
}
 