namespace SmartHealthMonitoring.Models;

public class SensorDataModel
{
    public DateTime Timestamp { get; set; }
    public int PulseRate { get; set; }
    public double BodyTemperature { get; set; }
    public double RoomTemperature { get; set; }
    public double RoomHumidity { get; set; }
}