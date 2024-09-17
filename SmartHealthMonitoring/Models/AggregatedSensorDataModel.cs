namespace SmartHealthMonitoring.Models;

public class AggregatedSensorDataModel
{
    public SensorModel Sensor { get; set; }
    public PatientModel Patient { get; set; }
    public SensorNodeModel SensorNode { get; set; }
    public List<SensorDataModel> SensorData { get; set; }
}