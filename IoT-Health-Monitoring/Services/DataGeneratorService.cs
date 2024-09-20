namespace IoT_Health_Monitoring.Services
{
    public class DataGeneratorService
    {
        private static readonly Random random = new Random();

        public Model.Simulator.SensorDataModel GenerateRandomSensorData(Guid sensorNodeId)
        {
            return new Model.Simulator.SensorDataModel
            {
                SensorNodeId = sensorNodeId.ToString(),
                TimeStamp = DateTime.UtcNow,
                BodyTemperature = GetRandomBodyTemperature(),
                PulseRate = GetRandomPulseRate(),
                RoomTemperature = GetRandomRoomTemperature(),
                RoomHumidity = GetRandomRoomHumidity()
            };
        }

        private double GetRandomBodyTemperature()
        {
            return Math.Round(30 + random.NextDouble() * (45 - 30), 1); 
        }

        private int GetRandomPulseRate()
        {
            return random.Next(30, 141);
        }

        private double GetRandomRoomTemperature()
        {
            return Math.Round(12 + random.NextDouble() * (45 - 12), 1);
        }

        private double GetRandomRoomHumidity()
        {
            return Math.Round(20 + random.NextDouble() * (80 - 20), 1);
        }

        public static readonly Guid[] SensorNodeIds =
        {
            new Guid("a1b2c3d4-e5f6-7890-1234-567890abcdef"),
            new Guid("b2c3d4e5-f678-9012-3456-7890abcdef12"),
            new Guid("c3d4e5f6-7890-1234-5678-90abcdef1234"),
            new Guid("d4e5f678-9012-3456-7890-abcdef123456"),
            new Guid("e5f67890-1234-5678-90ab-cdef12345678"),
            new Guid("f6789012-3456-7890-abcd-ef1234567890"),
            new Guid("01234567-89ab-cdef-1234-567890abcdef"),
            new Guid("12345678-90ab-cdef-1234-567890abcdef"),
            new Guid("23456789-0abc-def1-2345-67890abcdef1"),
            new Guid("34567890-1bcd-ef23-4567-890abcdef123")
        };
    }
}
