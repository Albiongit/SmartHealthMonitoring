import logging
from pyspark.sql import SparkSession
from pyspark.sql.functions import *
from pyspark.sql.types import StructType, StructField, StringType, IntegerType, FloatType
import uuid

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

logger.info("Starting spark session...")

spark = SparkSession.builder \
    .appName("KafkaSparkCassandra") \
    .config("spark.cassandra.connection.host", "cassandra") \
    .config("spark.cassandra.output.batch.size.bytes", "16000") \
    .config("spark.cassandra.output.batch.size.rows", "50") \
    .config("spark.cassandra.output.batch.grouping.buffer.size", "100") \
    .config("spark.jars.packages", "org.apache.spark:spark-sql-kafka-0-10_2.12:3.3.1,com.datastax.spark:spark-cassandra-connector_2.12:3.3.0") \
    .getOrCreate()

logger.info("Spark Session started.")

sensorDataSchema = StructType([
    StructField("sensor_node_id", StringType(), True),  
    StructField("time_stamp", StringType(), True),
    StructField("pulse_rate", IntegerType(), True),      
    StructField("body_temperature", FloatType(), True), 
    StructField("room_temperature", FloatType(), True), 
    StructField("room_humidity", FloatType(), True)
])

logger.info("Reading from Kafka topic...")

df = spark \
    .readStream \
    .format("kafka") \
    .option("kafka.bootstrap.servers", "kafka:9092") \
    .option("subscribe", "sensor-topic") \
    .load() \
    .selectExpr("CAST(value AS STRING) as json_string")

logger.info("Reading messages from Kafka topic kafka:9092")

df = df.select(from_json(col("json_string"), sensorDataSchema).alias("data")).select("data.*")
logger.info(df)

df = df.withColumn("time_stamp", to_timestamp(col("time_stamp"), "yyyy-MM-dd HH:mm:ss"))

generate_uuid = udf(lambda: str(uuid.uuid4()), StringType())

def generate_alarms(batch_df):
    alarms = []

    alarm_conditions = [
        ("body_temperature", 35.5, 37.0, "Temperature is too low", "Temperature is too high"),
        ("room_temperature", 20.0, 24.0, "Room temperature is too low", "Room temperature is too high"),
        ("room_humidity", 30.0, 60.0, "Room humidity is too low", "Room humidity is too high"),
        ("pulse_rate", 60, 100, "Pulse rate is too low", "Pulse rate is too high")
    ]

    for column, low_threshold, high_threshold, low_msg, high_msg in alarm_conditions:
        temp_alarms = batch_df \
            .withColumn("alarm_description",
                        when(col(column) < low_threshold, low_msg)
                        .when(col(column) > high_threshold, high_msg)) \
            .filter(col("alarm_description").isNotNull()) \
            .withColumn("alarm_id", generate_uuid()) \
            .withColumn("alarm_cause", lit(column)) \
            .withColumn("alarm_cause_value", col(column)) \
            .select("alarm_id", "time_stamp", "alarm_cause", "alarm_cause_value", "alarm_description", "sensor_node_id")

        logger.info(f"Printing alarms for condition {column[0]}:")
        temp_alarms.show()

        if temp_alarms.count() > 0:
            alarms.append(temp_alarms)
        else:
            logger.info(f"No alarms generated for {column}.")

    print(alarms)
    
    if alarms:
        combined_alarms = alarms[0]
        for alarm_df in alarms[1:]:
            combined_alarms = combined_alarms.union(alarm_df)
            print("combined alarms")
            combined_alarms.show(truncate=False)
        return combined_alarms
    return None

def process_batch(batch_df, batch_id):
    batch_df.persist()

    try:
        logger.info(f"Processing batch {batch_id}")
        logger.info(f"Number of rows before writing to Cassandra: {batch_df.count()}")

        batch_df.write \
            .format("org.apache.spark.sql.cassandra") \
            .mode("append") \
            .options(keyspace="health_data", table="sensor_data") \
            .save()

        logger.info(f"Sensor data for batch {batch_id} inserted into sensor_data table successfully.")

        combined_alarms = generate_alarms(batch_df)

        if combined_alarms and combined_alarms.count() > 0:
            logger.info(f"Found alarms in batch {batch_id}, inserting into alarm table.")
            combined_alarms.write \
                .format("org.apache.spark.sql.cassandra") \
                .mode("append") \
                .options(keyspace="health_data", table="alarm") \
                .save()
            logger.info(f"Alarms for batch {batch_id} processed successfully.")
        else:
            logger.info(f"No alarms found in batch {batch_id}.")

    except Exception as e:
        logger.error(f"Error processing batch {batch_id}: {e}")
    batch_df.unpersist()


query = df.writeStream \
    .outputMode("append") \
    .foreachBatch(process_batch) \
    .option("checkpointLocation", "/tmp/spark/checkpoints_sensor") \
    .option("maxOffsetsPerTrigger", "1000") \
    .start()

query.awaitTermination()