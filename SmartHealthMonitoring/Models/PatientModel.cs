namespace SmartHealthMonitoring.Models;

public class PatientModel
{
    public Guid PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
}