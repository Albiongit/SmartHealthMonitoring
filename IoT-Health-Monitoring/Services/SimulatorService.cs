using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Newtonsoft.Json;

namespace IoT_Health_Monitoring.Services
{
    public class SimulatorService
    {
        private readonly string bootstrapServers = "localhost:29092";
        private readonly string topic = "sensor-topic";

        DataGeneratorService dataGeneratorService;
        public SimulatorService(DataGeneratorService dataGeneratorService)
        {
            this.dataGeneratorService = dataGeneratorService;
        }

        public async Task<string> ProduceMessageAsync(int nrOfRows, CancellationToken cancellationToken)
        {
            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {

                try
                {
                    await EnsureTopicExistsAsync();

                    int totalSensorNodes = DataGeneratorService.SensorNodeIds.Length;
                    int rowsPerNode = nrOfRows / totalSensorNodes;
                    int remainder = nrOfRows % totalSensorNodes;

                    for (int nodeIndex = 0; nodeIndex < totalSensorNodes; nodeIndex++)
                    {
                        int rowsForThisNode = rowsPerNode;

                        if (nodeIndex < remainder)
                        {
                            rowsForThisNode++;
                        }

                        Guid currentSensorNodeId = DataGeneratorService.SensorNodeIds[nodeIndex];

                        List<Task> tasks = new List<Task>();

                        for (int i = 0; i < rowsForThisNode; i++)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                Console.WriteLine("Simulation stopped due to cancellation.");
                                break;
                            }

                            Model.Simulator.SensorDataModel row = dataGeneratorService.GenerateRandomSensorData(currentSensorNodeId);
                            row.TimeStamp = row.TimeStamp.AddMinutes(i);

                            string jsonString = JsonConvert.SerializeObject(row, new JsonSerializerSettings { DateFormatString = "yyyy-MM-dd HH:mm:ss" });

                            tasks.Add(producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonString }));
                        }

                        await Task.WhenAll(tasks).WaitAsync(cancellationToken);
                    }

                    return "Data generated successfully!";

                }
                catch (OperationCanceledException)
                {
                    return "Operation was canceled.";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Data generation failed: {e.Error.Reason}");
                    return e.Error.Reason;
                }
            }
        }

        public async Task EnsureTopicExistsAsync()
        {
            var config = new AdminClientConfig
            {
                BootstrapServers = bootstrapServers
            };

            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    var existingTopics = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                    var topicNames = existingTopics.Topics.Select(t => t.Topic).ToList();

                    if (!topicNames.Contains(topic))
                    {
                        var topicSpecifications = new TopicSpecification[]
                        {
                        new TopicSpecification
                        {
                            Name = topic,
                            NumPartitions = 1,
                            ReplicationFactor = 1
                        }
                        };

                        await adminClient.CreateTopicsAsync(topicSpecifications);
                        Console.WriteLine($"Topic '{topic}' created.");
                    }
                    else
                    {
                        Console.WriteLine($"Topic '{topic}' already exists.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error occurred: {e.Message}");
                }
            }
        }
    }
}
