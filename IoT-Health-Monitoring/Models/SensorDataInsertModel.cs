using TinyCsvParser.Mapping;

namespace IoT_Health_Monitoring.Models
{
    public class SensorDataInsertModel
    {
        public Guid SensorNodeId { get; set; }
        public DateTime Timestamp { get; set; }
        public double BodyTemperature { get; set; }
        public int PulseRate { get; set; }
        public double RoomHumidity { get; set; }
        public double RoomTemperature { get; set; }
    }

    public class CsvSensorDataInsertModelMapping : CsvMapping<SensorDataInsertModel>
    {
        public CsvSensorDataInsertModelMapping()
            : base()
        {
            MapProperty(0, x => x.SensorNodeId);
            MapProperty(1, x => x.Timestamp);
            MapProperty(2, x => x.BodyTemperature);
            MapProperty(3, x => x.PulseRate);
            MapProperty(4, x => x.RoomHumidity);
            MapProperty(5, x => x.RoomTemperature);
        }
    }
}