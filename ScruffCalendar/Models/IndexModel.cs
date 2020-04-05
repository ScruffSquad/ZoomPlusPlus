﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ScruffCalendar.Models
{
    public class IndexModel : PageModel
    {
        public string ZoomFirstName { get; set; }

        public string ZoomLastName { get; set; }

        public string ZoomEmail { get; set; }

        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                ZoomFirstName = User.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value;
                ZoomLastName = User.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value;
                ZoomEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            }
        }
    }
}