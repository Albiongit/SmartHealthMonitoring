using TinyCsvParser.Mapping;

namespace IoT_Health_Monitoring.Models
{
    public class AlarmInsertModel
    {
        public Guid AlarmId { get; set; }
        public string AlarmCause { get; set; } = null!;
        public string AlarmCauseValue { get; set; } = null!;
        public string AlarmDescription { get; set; } = null!;
        public Guid SensorNodeId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class CsvAlarmInsertModelMapping : CsvMapping<AlarmInsertModel>
    {
        public CsvAlarmInsertModelMapping()
            : base()
        {
            MapProperty(0, x => x.AlarmId);
            MapProperty(1, x => x.AlarmCause);
            MapProperty(2, x => x.AlarmCauseValue);
            MapProperty(3, x => x.AlarmDescription);
            MapProperty(4, x => x.SensorNodeId);
            MapProperty(5, x => x.TimeStamp);
        }
    }
}