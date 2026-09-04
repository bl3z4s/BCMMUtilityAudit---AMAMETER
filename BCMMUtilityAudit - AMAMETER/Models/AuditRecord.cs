using SQLite;

namespace BCMMUtilityAudit___AMAMETER.Models
{
    public class AuditRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AccountNo { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string MeterReading { get; set; } = string.Empty;
        public double BilledReading { get; set; }
        public double ActualReading { get; set; }
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string GpsCoords { get; set; } = string.Empty;
        public string LocalImagePath { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string PdfPath { get; set; } = string.Empty;
    }
}