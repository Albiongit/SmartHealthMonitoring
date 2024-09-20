namespace IoT_Health_Monitoring.Models
{
    public class PatientModel
    {
        public Guid PatientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime Birthday { get; set; }
        public string Gender { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
