namespace SmartHealthMonitoring.Models;

public class SensorNodeModel
{
    public Guid NodeId { get; set; }
    public string NodeName { get; set; }
    public int BatteryPercentage { get; set; }
    public string HospitalName { get; set; }
    public Guid PatientId { get; set; }
    public string SensorCode { get; set; }
}