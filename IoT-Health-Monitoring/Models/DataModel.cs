namespace IoT_Health_Monitoring.Models
{
    public class DataModel
    {
        public SensorModel Sensor { get; set; } = null!;
        public PatientModel Patient { get; set; } = null!;
        public SensorNodeModel SensorNode { get; set; } = null!;
        public List<SensorDataModel> SensorData { get; set; } = null!;
    }
}
