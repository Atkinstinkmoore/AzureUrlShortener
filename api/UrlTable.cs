using System;
using Newtonsoft.Json;

namespace api
{
    public class UrlTable
    {
        public string id { get; set; }
        public string longUrl { get; set; }
        public string createdBy { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public int timesClicked { get; set; }
    }
}
