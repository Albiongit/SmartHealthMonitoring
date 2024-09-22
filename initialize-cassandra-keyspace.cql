-- Create Keyspace
CREATE KEYSPACE IF NOT EXISTS health_data 
WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 1};

-- Use the Keyspace
USE health_data;

-- Create Aggregated Parking Data Table
CREATE TABLE IF NOT EXISTS sensor (
    sensor_code TEXT,          
    sensor_name TEXT,          
    manufacturer TEXT,        
    PRIMARY KEY (sensor_code)  
);

CREATE TABLE IF NOT EXISTS patient (
    patient_id UUID,           
    first_name TEXT,           
    last_name TEXT,           
    birthday DATE,             
    gender TEXT,               
    address TEXT,             
    PRIMARY KEY (patient_id)   
);

CREATE TABLE IF NOT EXISTS sensor_node (
    node_id UUID,                
    node_name TEXT,            
    battery_percentage INT, 
    hospital_name TEXT,       
    patient_id UUID,            
    sensor_code TEXT,           
    PRIMARY KEY (node_id)       
);

CREATE TABLE IF NOT EXISTS sensor_data (
    sensor_node_id UUID,    
	time_stamp TIMESTAMP, 
	pulse_rate INT,     
    body_temperature FLOAT,    
	room_temperature FLOAT,	
    room_humidity FLOAT,
    PRIMARY KEY (sensor_node_id, time_stamp) 
);

CREATE TABLE IF NOT EXISTS alarm (
    alarm_id UUID,      
	time_stamp TIMESTAMP,
	alarm_cause TEXT,  
	alarm_cause_value TEXT,  
    alarm_description TEXT,            
	sensor_node_id UUID,
    PRIMARY KEY (alarm_id)       
);

INSERT INTO health_data.sensor (sensor_code, sensor_name, manufacturer)
VALUES ('SENS001', 'Vital Sign Monitor', 'MedTech Inc.');
INSERT INTO health_data.sensor (sensor_code, sensor_name, manufacturer)
VALUES ('SENS002', 'Health Monitoring Device', 'HealthCorp Ltd.');
INSERT INTO health_data.sensor (sensor_code, sensor_name, manufacturer)
VALUES ('SENS003', 'Environmental Sensor', 'BioHealth Devices');
INSERT INTO health_data.sensor (sensor_code, sensor_name, manufacturer)
VALUES ('SENS004', 'Multi-Parameter Sensor', 'WellBeing Technologies');
INSERT INTO health_data.sensor (sensor_code, sensor_name, manufacturer)
VALUES ('SENS005', 'Advanced Health Monitor', 'SmartHealth Systems');

INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (11111111-1111-1111-1111-111111111111, 'John', 'Doe', '1985-07-15', 'M', '123 Health St, City A');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (22222222-2222-2222-2222-222222222222, 'Jane', 'Smith', '1990-04-23', 'F', '456 Care Ave, City B');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (33333333-3333-3333-3333-333333333333, 'Michael', 'Johnson', '1978-09-30', 'M', '789 Wellness Blvd, City C');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (44444444-4444-4444-4444-444444444444, 'Emily', 'Clark', '1995-02-18', 'F', '101 Treatment Rd, City D');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (55555555-5555-5555-5555-555555555555, 'Daniel', 'Garcia', '1982-11-05', 'M', '202 Recovery St, City E');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (66666666-6666-6666-6666-666666666666, 'Sophia', 'Martinez', '1988-03-12', 'F', '303 Health Way, City F');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (77777777-7777-7777-7777-777777777777, 'David', 'Brown', '1975-06-25', 'M', '404 Care Cir, City G');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (88888888-8888-8888-8888-888888888888, 'Olivia', 'Davis', '1984-08-14', 'F', '505 Wellness Ln, City H');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (99999999-9999-9999-9999-999999999999, 'James', 'Wilson', '1992-01-09', 'M', '606 Recovery Rd, City I');
INSERT INTO health_data.patient (patient_id, first_name, last_name, birthday, gender, address)
VALUES (aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa, 'Liam', 'Lee', '1981-05-03', 'M', '707 Health St, City J');

INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (a1b2c3d4-e5f6-7890-1234-567890abcdef, 'Node A', 85, 'General Hospital', 11111111-1111-1111-1111-111111111111, 'SENS001');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (b2c3d4e5-f678-9012-3456-7890abcdef12, 'Node B', 92, 'General Hospital', 22222222-2222-2222-2222-222222222222, 'SENS002');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (c3d4e5f6-7890-1234-5678-90abcdef1234, 'Node C', 77, 'City Clinic', 33333333-3333-3333-3333-333333333333, 'SENS003');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (d4e5f678-9012-3456-7890-abcdef123456, 'Node D', 65, 'Wellness Center', 44444444-4444-4444-4444-444444444444, 'SENS004');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (e5f67890-1234-5678-90ab-cdef12345678, 'Node E', 80, 'CarePoint Hospital', 55555555-5555-5555-5555-555555555555, 'SENS005');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (f6789012-3456-7890-abcd-ef1234567890, 'Node F', 90, 'Recovery Hospital', 66666666-6666-6666-6666-666666666666, 'SENS001');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (01234567-89ab-cdef-1234-567890abcdef, 'Node G', 70, 'CarePoint Hospital', 77777777-7777-7777-7777-777777777777, 'SENS002');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (12345678-90ab-cdef-1234-567890abcdef, 'Node H', 85, 'City Clinic', 88888888-8888-8888-8888-888888888888, 'SENS003');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (23456789-0abc-def1-2345-67890abcdef1, 'Node I', 60, 'Wellness Center', 99999999-9999-9999-9999-999999999999, 'SENS004');
INSERT INTO health_data.sensor_node (node_id, node_name, battery_percentage, hospital_name, patient_id, sensor_code)
VALUES (34567890-1bcd-ef23-4567-890abcdef123, 'Node J', 95, 'General Hospital', aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa, 'SENS005');
