namespace IoT_Health_Monitoring.Models
{
    public class SensorNodeModel
    {
        public Guid NodeId { get; set; }
        public string NodeName { get; set; } = null!;
        public int BatteryPercentage { get; set; }
        public string HospitalName { get; set; } = null!;
        public Guid PatientId { get; set; }
        public string SensorCode { get; set; } = null!;
    }
}
