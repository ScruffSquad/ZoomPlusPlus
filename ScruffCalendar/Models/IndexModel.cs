using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScruffCalendar.Zoom;

namespace ScruffCalendar.Models
{
    public class IndexModel : PageModel
    {
        public string ZoomUserId { get; set; }

        public string ZoomFirstName { get; set; }

        public string ZoomLastName { get; set; }

        public string ZoomEmail { get; set; }

        public Meeting[] Meetings { get; set; }
    }
}
