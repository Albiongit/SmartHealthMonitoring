export class MasterData {
      private static sensors: SensorModel[] = [
        { sensorCode: 'SENS001', sensorName: 'Vital Sign Monitor', manufacturer: 'MedTech Inc.' },
        { sensorCode: 'SENS002', sensorName: 'Health Monitoring Device', manufacturer: 'HealthCorp Ltd.' },
        { sensorCode: 'SENS003', sensorName: 'Environmental Sensor', manufacturer: 'BioHealth Devices' },
        { sensorCode: 'SENS004', sensorName: 'Multi-Parameter Sensor', manufacturer: 'WellBeing Technologies' },
        { sensorCode: 'SENS005', sensorName: 'Advanced Health Monitor', manufacturer: 'SmartHealth Systems' },
      ];
    
      private static patients: PatientModel[] = [
        { patientId: '11111111-1111-1111-1111-111111111111', firstName: 'John', lastName: 'Doe', birthday: new Date('1985-07-15'), gender: 'M', address: '123 Health St, City A' },
        { patientId: '22222222-2222-2222-2222-222222222222', firstName: 'Jane', lastName: 'Smith', birthday: new Date('1990-04-23'), gender: 'F', address: '456 Care Ave, City B' },
        { patientId: '33333333-3333-3333-3333-333333333333', firstName: 'Michael', lastName: 'Johnson', birthday: new Date('1978-09-30'), gender: 'M', address: '789 Wellness Blvd, City C' },
        { patientId: '44444444-4444-4444-4444-444444444444', firstName: 'Emily', lastName: 'Clark', birthday: new Date('1995-02-18'), gender: 'F', address: '101 Treatment Rd, City D' },
        { patientId: '55555555-5555-5555-5555-555555555555', firstName: 'Daniel', lastName: 'Garcia', birthday: new Date('1982-11-05'), gender: 'M', address: '202 Recovery St, City E' },
        { patientId: '66666666-6666-6666-6666-666666666666', firstName: 'Sophia', lastName: 'Martinez', birthday: new Date('1988-03-12'), gender: 'F', address: '303 Health Way, City F' },
        { patientId: '77777777-7777-7777-7777-777777777777', firstName: 'David', lastName: 'Brown', birthday: new Date('1975-06-25'), gender: 'M', address: '404 Care Cir, City G' },
        { patientId: '88888888-8888-8888-8888-888888888888', firstName: 'Olivia', lastName: 'Davis', birthday: new Date('1984-08-14'), gender: 'F', address: '505 Wellness Ln, City H' },
        { patientId: '99999999-9999-9999-9999-999999999999', firstName: 'James', lastName: 'Wilson', birthday: new Date('1992-01-09'), gender: 'M', address: '606 Recovery Rd, City I' },
        { patientId: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', firstName: 'Liam', lastName: 'Lee', birthday: new Date('1981-05-03'), gender: 'M', address: '707 Health St, City J' },
      ];
    
      private static sensorNodes: SensorNodeModel[] = [
        { nodeId: 'a1b2c3d4-e5f6-7890-1234-567890abcdef', nodeName: 'Node A', batteryPercentage: 85, hospitalName: 'General Hospital', patientId: '11111111-1111-1111-1111-111111111111', sensorCode: 'SENS001' },
        { nodeId: 'b2c3d4e5-f678-9012-3456-7890abcdef12', nodeName: 'Node B', batteryPercentage: 92, hospitalName: 'General Hospital', patientId: '22222222-2222-2222-2222-222222222222', sensorCode: 'SENS002' },
        { nodeId: 'c3d4e5f6-7890-1234-5678-90abcdef1234', nodeName: 'Node C', batteryPercentage: 77, hospitalName: 'City Clinic', patientId: '33333333-3333-3333-3333-333333333333', sensorCode: 'SENS003' },
        { nodeId: 'd4e5f678-9012-3456-7890-abcdef123456', nodeName: 'Node D', batteryPercentage: 65, hospitalName: 'Wellness Center', patientId: '44444444-4444-4444-4444-444444444444', sensorCode: 'SENS004' },
        { nodeId: 'e5f67890-1234-5678-90ab-cdef12345678', nodeName: 'Node E', batteryPercentage: 80, hospitalName: 'CarePoint Hospital', patientId: '55555555-5555-5555-5555-555555555555', sensorCode: 'SENS005' },
        { nodeId: 'f6789012-3456-7890-abcd-ef1234567890', nodeName: 'Node F', batteryPercentage: 90, hospitalName: 'Recovery Hospital', patientId: '66666666-6666-6666-6666-666666666666', sensorCode: 'SENS001' },
        { nodeId: '01234567-89ab-cdef-1234-567890abcdef', nodeName: 'Node G', batteryPercentage: 70, hospitalName: 'CarePoint Hospital', patientId: '77777777-7777-7777-7777-777777777777', sensorCode: 'SENS002' },
        { nodeId: '12345678-90ab-cdef-1234-567890abcdef', nodeName: 'Node H', batteryPercentage: 85, hospitalName: 'City Clinic', patientId: '88888888-8888-8888-8888-888888888888', sensorCode: 'SENS003' },
        { nodeId: '23456789-0abc-def1-2345-67890abcdef1', nodeName: 'Node I', batteryPercentage: 60, hospitalName: 'Wellness Center', patientId: '99999999-9999-9999-9999-999999999999', sensorCode: 'SENS004' },
        { nodeId: '34567890-1bcd-ef23-4567-890abcdef123', nodeName: 'Node J', batteryPercentage: 95, hospitalName: 'General Hospital', patientId: 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', sensorCode: 'SENS005' },
      ];
    
      static getSensors(): SensorModel[] {
        return this.sensors;
      }
    
      static getPatients(): PatientModel[] {
        return this.patients;
      }
    
      static getSensorNodes(): SensorNodeModel[] {
        return this.sensorNodes;
      }
}

interface SensorDataModel {
    timestamp: Date;
    pulseRate: number;
    bodyTemperature: number;
    roomTemperature: number;
    roomHumidity: number;
  }
  
  interface PatientModel {
    patientId: string;
    firstName: string;
    lastName: string;
    birthday: Date;
    gender: string;
    address: string;
  }
  
  interface SensorModel {
    sensorCode: string;
    sensorName: string;
    manufacturer: string;
  }
  
  interface SensorNodeModel {
    nodeId: string;
    nodeName: string;
    batteryPercentage: number;
    hospitalName: string;
    patientId: string;
    sensorCode: string;
  }