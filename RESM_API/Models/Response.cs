using RESM_API.Model;

namespace RestAPI_farmersMarket.Models
{
    public class Response
    {
        public int statusCode { get; set; }
        public string? statusMessage { get; set; }
        public Property? property { get; set; }
        public List<Property>? properties { get; set; }
    }
}
