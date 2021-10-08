using System;
using Newtonsoft.Json;

namespace api
{
    public class UrlTable
    {
        public string Id { get; set; }
        public string LongUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int TimesClicked { get; set; }
    }
}
