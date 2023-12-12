namespace Cloud_API.Models
{
    public class PollResponse
    {
        public bool jobReady { get; set; }
        public List<string> mediaTypes { get; set; } = null!;
        public string? jobToken { get; set; }
        public string? deleteMethod { get; set; }
        public List<ClientAction>? clientAction { get; set; }
        public List<string>? claimBarcodeReader { get; set; }
        public List<string>? claimKeyboard { get; set; }
        public List<Display>? display { get; set; }
        public string? jobGetUrl { get; set; } = "https://192.168.1.159:45455/api/cloudprnt/";
        public string? jobConfirmationUrl { get; set; }
    }

}
