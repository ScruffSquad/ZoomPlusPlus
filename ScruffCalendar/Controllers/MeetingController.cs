using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScruffCalendar.Models;
using ScruffCalendar.Zoom;

namespace ScruffCalendar.Controllers
{
    public class MeetingController : Controller
    {
        // GET: Meeting
        public ActionResult Index()
        {
            ViewData["pmi"] = User.FindFirst(c => c.Type == "urn:zoom:pmi")?.Value;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Detail(string meetingId)
        {
            meetingId = meetingId.Replace("-", "");
            if (!long.TryParse(meetingId, out long id))
            {
                return BadRequest();
            }

            var zoom = new ZoomClient(HttpContext);

            Meeting meeting = await zoom.GetMeetingAsync(id);

            return View(meeting);
        }

        [HttpPost]
        public async Task<ActionResult> List(string popUser, string popPass)
        {
            var client = new gmailScraper(popUser, popPass);

            client.connect();

            var meetings = client.GetMeetings();

            return View(meetings);
        }
    }
}