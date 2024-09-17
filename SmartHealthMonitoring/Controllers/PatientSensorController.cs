using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Models;
using SmartHealthMonitoring.Services;

namespace SmartHealthMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientSensorController : ControllerBase
    {
        private readonly PatientSensorService patientSensorService;

        public PatientSensorController(PatientSensorService patientSensorService)
        {
            this.patientSensorService = patientSensorService;
        }

        [HttpGet("generate")]
        public ActionResult<PatientSensorDataModel> GenerateSensorData()
        {
            var sensorData = patientSensorService.GenerateRandomSensorData();

            return Ok(sensorData);
        }
    }
}