using System.Text;
using Cassandra;
using IoT_Health_Monitoring.Models;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace IoT_Health_Monitoring.Services
{
    public class DataService
    {
        private readonly Cassandra.ISession cassandraSession;

        public DataService(Cassandra.ISession cassandraSession)
        {
            this.cassandraSession = cassandraSession;
        }

        public async Task<List<DataModel?>> GetAggregatedSensorDataAsync()
        {
            List<Task<DataModel?>> tasks = new List<Task<DataModel?>>();

            foreach (Guid id in DataGeneratorService.SensorNodeIds)
            {
                tasks.Add(GetSensorNodeDataAsync(id));
            }

            return (await Task.WhenAll(tasks)).ToList();
        }

        private async Task<DataModel?> GetSensorNodeDataAsync(Guid sensorNodeId)
        {
            SensorNodeModel? sensorNode = await GetSensorNodeAsync(sensorNodeId);

            if (sensorNode == null) return null;

            PatientModel? patient = await GetPatientAsync(sensorNode.PatientId);
            SensorModel? sensor = await GetSensorAsync(sensorNode.SensorCode);

            List<SensorDataModel> sensorDataList = await GetSensorDataAsync(sensorNodeId);

            return new DataModel
            {
                Sensor = sensor!,
                Patient = patient!,
                SensorNode = sensorNode,
                SensorData = sensorDataList,
            };
        }

        private async Task<SensorNodeModel?> GetSensorNodeAsync(Guid sensorNodeId)
        {
            string query = "SELECT * FROM sensor_node WHERE node_id = ?";

            RowSet? resultSet = await cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorNodeId));
            Row? row = resultSet.FirstOrDefault();

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

        private async Task<PatientModel?> GetPatientAsync(Guid patientId)
        {
            string query = "SELECT * FROM patient WHERE patient_id = ?";

            RowSet? resultSet = await cassandraSession.ExecuteAsync(new SimpleStatement(query, patientId));
            Row? row = resultSet.FirstOrDefault();

            if (row != null)
            {
                LocalDate birthday = row.GetValue<LocalDate>("birthday");

                return new PatientModel
                {
                    PatientId = row.GetValue<Guid>("patient_id"),
                    FirstName = row.GetValue<string>("first_name"),
                    LastName = row.GetValue<string>("last_name"),
                    Birthday = new DateTime(birthday.Year, birthday.Month, birthday.Day),
                    Gender = row.GetValue<string>("gender"),
                    Address = row.GetValue<string>("address")
                };
            }
            return null;
        }

        private async Task<SensorModel?> GetSensorAsync(string sensorCode)
        {
            string query = "SELECT * FROM sensor WHERE sensor_code = ?";

            RowSet? resultSet = await cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorCode));
            Row? row = resultSet.FirstOrDefault();

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

        private async Task<List<SensorDataModel>> GetSensorDataAsync(Guid sensorNodeId)
        {
            string query = "SELECT * FROM sensor_data WHERE sensor_node_id = ? ORDER BY time_stamp DESC";

            RowSet? resultSet = await cassandraSession.ExecuteAsync(new SimpleStatement(query, sensorNodeId));
            List<SensorDataModel> sensorDataList = new List<SensorDataModel>();

            foreach (Row? row in resultSet)
            {
                DateTime timestamp = row.GetValue<DateTime>("time_stamp");

                sensorDataList.Add(new SensorDataModel
                {
                    Timestamp = new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second),
                    PulseRate = row.GetValue<int>("pulse_rate"),
                    BodyTemperature = (double)row.GetValue<float>("body_temperature"),
                    RoomTemperature = (double)row.GetValue<float>("room_temperature"),
                    RoomHumidity = (double)row.GetValue<float>("room_humidity")
                });
            }

            return sensorDataList;
        }

        public async Task InsertSensorDataBatchAsync(string filePath)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvSensorDataInsertModelMapping csvMapper = new CsvSensorDataInsertModelMapping();
            CsvParser<SensorDataInsertModel> csvParser = new CsvParser<SensorDataInsertModel>(csvParserOptions, csvMapper);

            List<CsvMappingResult<SensorDataInsertModel>> result = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            List<SensorDataInsertModel> records = new List<SensorDataInsertModel>();

            foreach (CsvMappingResult<SensorDataInsertModel> record in result)
            {
                if (record.IsValid)
                {
                    records.Add(record.Result);
                }
                else
                {
                    Console.WriteLine($"Error: {record.Error}");
                }
            }

            const int batchSize = 200;
            List<Task> batchTasks = new List<Task>();

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = new BatchStatement();

                for (int j = i; j < i + batchSize && j < records.Count; j++)
                {
                    var record = records[j];
                    string query = "INSERT INTO sensor_data (sensor_node_id, time_stamp, body_temperature, pulse_rate, room_humidity, room_temperature) " +
                                   "VALUES (?, ?, ?, ?, ?, ?)";

                    SimpleStatement statement = new SimpleStatement(query,
                        record.SensorNodeId,
                        record.Timestamp,
                        (float)record.BodyTemperature,
                        record.PulseRate,
                        (float)record.RoomHumidity,
                        (float)record.RoomTemperature);

                    batch.Add(statement);
                }

                if (!batch.IsEmpty)
                {
                    batchTasks.Add(cassandraSession.ExecuteAsync(batch));
                }
            }

            await Task.WhenAll(batchTasks);
        }
        public async Task InsertAlarmsBatchAsync(string filePath)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            CsvAlarmInsertModelMapping csvMapper = new CsvAlarmInsertModelMapping();
            CsvParser<AlarmInsertModel> csvParser = new CsvParser<AlarmInsertModel>(csvParserOptions, csvMapper);

            List<CsvMappingResult<AlarmInsertModel>> result = csvParser.ReadFromFile(filePath, Encoding.ASCII).ToList();

            List<AlarmInsertModel> records = new List<AlarmInsertModel>();

            foreach (CsvMappingResult<AlarmInsertModel> record in result)
            {
                if (record.IsValid)
                {
                    records.Add(record.Result);
                }
                else
                {
                    Console.WriteLine($"Error: {record.Error}");
                }
            }

            const int batchSize = 200;
            List<Task> batchTasks = new List<Task>();

            for (int i = 0; i < records.Count; i += batchSize)
            {
                var batch = new BatchStatement();

                for (int j = i; j < i + batchSize && j < records.Count; j++)
                {
                    var record = records[j];
                    string query = "INSERT INTO alarm (alarm_id, alarm_cause, alarm_cause_value, alarm_description, sensor_node_id, time_stamp) " +
                                   "VALUES (?, ?, ?, ?, ?, ?)";

                    SimpleStatement statement = new SimpleStatement(query,
                        record.AlarmId,
                        record.AlarmCause,
                        record.AlarmCauseValue,
                        record.AlarmDescription,
                        record.SensorNodeId,
                        record.TimeStamp);

                    batch.Add(statement);
                }

                if (!batch.IsEmpty)
                {
                    batchTasks.Add(cassandraSession.ExecuteAsync(batch));
                }
            }

            await Task.WhenAll(batchTasks);
        }
    }
}
