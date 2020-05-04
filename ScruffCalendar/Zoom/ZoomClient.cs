using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using ScruffCalendar.Models;

namespace ScruffCalendar.Zoom
{
    public class ZoomClient
    {
        private const string BaseUrl = "https://api.zoom.us/v2";

        private readonly HttpClient client = new HttpClient();
        private readonly HttpContext context;

        public ZoomClient(HttpContext context)
        {
            this.context = context;
        }

        private async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string path)
        {
            var request = new HttpRequestMessage(method, path);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await context.GetTokenAsync("access_token"));
            return request;
        }

        public async Task<Meeting> GetMeetingAsync(long meetingId)
        {
            var request = await CreateRequestAsync(HttpMethod.Get, $"{BaseUrl}/meetings/{meetingId}");
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Meeting>(stream);
        }

        public async Task<MeetingPage> ListMeetingsAsync(long pageSize = 30, long pageNumber = 1)
        {
            var request = await CreateRequestAsync(HttpMethod.Get, $"{BaseUrl}/users/me/meetings?type=upcoming&page_size={pageSize}&page_number={pageNumber}");
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync();
            var page = await JsonSerializer.DeserializeAsync<MeetingPage>(stream);
            return page;
        }
    }
}
