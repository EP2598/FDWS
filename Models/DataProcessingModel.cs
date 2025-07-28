using System;

namespace FDWS.Models
{
    public class DataProcessingModel
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
    }
}