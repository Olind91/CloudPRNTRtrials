using System;
using System.Collections.Generic;


namespace Cloud_API.Models
{
    public class PollRequest
    {
        public string status { get; set; } = null!;
        public string printerMAC { get; set; } = null!;
        public string? uniqueID { get; set; }
        public string? statusCode { get; set; }
        public string? jobToken { get; set; }
        public bool printingInProgress { get; set; }
        public List<ClientAction>? clientAction { get; set; }
        public List<BarcodeReader>? barcodeReader { get; set; }
        public List<Keyboard>? keyboard { get; set; }
        public List<Display>? display { get; set; }
    }

    public class ClientAction
    {
        public string request { get; set; }
        public string result { get; set; }
    }

    public class BarcodeReader
    {
        public string name { get; set; }
        public StatusInfo? status { get; set; }
        public List<Scan>? scan { get; set; }
    }

    public class Keyboard
    {
        public string? name { get; set; }
        public StatusInfo? status { get; set; }
        public string? keyPresses { get; set; }
    }

    public class Display
    {
        public string? name { get; set; }
        public StatusInfo? status { get; set; }
    }

    public class StatusInfo
    {
        public bool connected { get; set; }
        public bool claimed { get; set; }
    }

    public class Scan
    {
        public string? data { get; set; }
        public string? symbology { get; set; }
    }
}