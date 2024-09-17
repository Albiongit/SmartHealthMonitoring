using Cassandra;
using SmartHealthMonitoring.Models;

namespace SmartHealthMonitoring.Services;

public class AggregatedSensorDataService
{
    private readonly Cassandra.ISession _cassandraSession;

    public AggregatedSensorDataService(Cassandra.ISession cassandraSession)
    {
        _cassandraSession = cassandraSession;
    }

    public async Task<AggregatedSensorDataModel?> GetAggregatedSensorData(Guid sensorNodeId)
    {
        var sensorNode = await GetSensorNode(sensorNodeId);

        if (sensorNode == null) 
            return null;

        var patient = await GetPatient(sensorNode.PatientId);
        var sensor = await GetSensor(sensorNode.SensorCode);

        var sensorDataList = await GetSensorData(sensorNodeId);

        return new AggregatedSensorDataModel
        {
            Sensor = sensor,
            Patient = patient,
            SensorNode = sensorNode,
            SensorData = sensorDataList
        };
    }

    private async Task<SensorNodeModel?> GetSensorNode(Guid sensorNodeId)
    {
        var query = "SELECT * FROM sensor_node WHERE node_id = ?";
        var resultSet = await _cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorNodeId));

        // Map the result to SensorNodeModel
        var row = resultSet.FirstOrDefault();
        if (row != null)
        {
            return new SensorNodeModel
            {
                NodeId = row.GetValue<Guid>("node_id"),
                NodeName = row.GetValue<string>("node_name"),
                BatteryPercentage = row.GetValue<int>("battery_percentage"),
                HospitalName = row.GetValue<string>("hospital_name"),
                PatientId = row.GetValue<Guid>("patient_id"),
                SensorCode = row.GetValue<string>("sensor_code")
            };
        }

        return null;
    }

    private async Task<PatientModel?> GetPatient(Guid patientId)
    {
        var query = "SELECT * FROM patient WHERE patient_id = ?";
        var resultSet = await _cassandraSession.ExecuteAsync(new SimpleStatement(query, patientId));

        var row = resultSet.FirstOrDefault();
        if (row != null)
        {
            return new PatientModel
            {
                PatientId = row.GetValue<Guid>("patient_id"),
                FirstName = row.GetValue<string>("first_name"),
                LastName = row.GetValue<string>("last_name"),
                Birthday = row.GetValue<DateTime>("birthday"),
                Gender = row.GetValue<string>("gender"),
                Address = row.GetValue<string>("address")
            };
        }
        return null;
    }

    private async Task<SensorModel?> GetSensor(string sensorCode)
    {
        var query = "SELECT * FROM sensor WHERE sensor_code = ?";
        var resultSet = await _cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorCode));

        var row = resultSet.FirstOrDefault();
        if (row != null)
        {
            return new SensorModel
            {
                SensorCode = row.GetValue<string>("sensor_code"),
                SensorName = row.GetValue<string>("sensor_name"),
                Manufacturer = row.GetValue<string>("manufacturer")
            };
        }
        return null;
    }

    private async Task<List<SensorDataModel>> GetSensorData(Guid sensorNodeId)
    {
        var query = "SELECT * FROM sensor_data WHERE sensor_node_id = ? ORDER BY timestamp DESC";
        var resultSet = await _cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorNodeId));

        var sensorDataList = new List<SensorDataModel>();

        foreach (var row in resultSet)
        {
            sensorDataList.Add(new SensorDataModel
            {
                Timestamp = row.GetValue<DateTime>("timestamp"),
                PulseRate = row.GetValue<int>("pulse_rate"),
                BodyTemperature = row.GetValue<double>("body_temperature"),
                RoomTemperature = row.GetValue<double>("room_temperature"),
                RoomHumidity = row.GetValue<double>("room_humidity")
            });
        }

        return sensorDataList;
    }
}