using Google.Protobuf.WellKnownTypes;

namespace FeedbackAPI.Models
{
    public class GoogleCaptchaResponse
    {
        public bool success;
        public Timestamp hallenge_ts;
        public string hostname;
    }
}
