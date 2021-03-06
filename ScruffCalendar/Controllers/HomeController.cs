﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScruffCalendar.Models;
using ScruffCalendar.Zoom;

namespace ScruffCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            var zoom = new ZoomClient(HttpContext);

            string userId = User.FindFirst(c => c.Type == "urn:zoom:userId")?.Value;

            var model = new IndexModel()
            {
                Meetings = (await zoom.ListMeetingsAsync()).Meetings,
                ZoomUserId = userId,
                ZoomFirstName = User.FindFirst(c => c.Type == ClaimTypes.GivenName)?.Value,
                ZoomLastName = User.FindFirst(c => c.Type == ClaimTypes.Surname)?.Value,
                ZoomEmail = User.FindFirst(c => c.Type == ClaimTypes.Email)?.Value,
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}