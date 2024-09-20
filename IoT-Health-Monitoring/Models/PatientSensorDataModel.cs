namespace IoT_Health_Monitoring.Models
{
    public class PatientSensorDataModel
    {
        public Guid SensorNodeId { get; set; }
        public DateTime TimeStamp { get; set; }
        public double BodyTemperature { get; set; }
        public int PulseRate { get; set; }
        public double RoomTemperature { get; set; }
        public double RoomHumidity { get; set; }
    }
}
