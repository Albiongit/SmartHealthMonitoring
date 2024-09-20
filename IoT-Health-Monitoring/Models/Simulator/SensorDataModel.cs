using Newtonsoft.Json;

namespace IoT_Health_Monitoring.Model.Simulator
{
    public class SensorDataModel
    {
        [JsonProperty("sensor_node_id")]
        public string SensorNodeId { get; set; }

        [JsonProperty("time_stamp")]
        public DateTime TimeStamp { get; set; }

        [JsonProperty("pulse_rate")]
        public int PulseRate { get; set; }

        [JsonProperty("body_temperature")]
        public double BodyTemperature { get; set; }

        [JsonProperty("room_temperature")]
        public double RoomTemperature { get; set; }

        [JsonProperty("room_humidity")]
        public double RoomHumidity { get; set; }
    }
}
