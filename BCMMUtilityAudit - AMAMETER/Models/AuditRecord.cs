using SQLite;
using System;

namespace BCMMUtilityAudit___AMAMETER.Models
{
    public class AuditRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string MeterReading { get; set; } = string.Empty;
        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string LocalImagePath { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}