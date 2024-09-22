using IoT_Health_Monitoring.Models;
using IoT_Health_Monitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Health_Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly DataService dataService;

        public DataController(DataService dataService)
        {
            this.dataService = dataService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DataModel?>>> GetAggregatedDataAsync()
        {
            var aggregatedData = await dataService.GetAggregatedSensorDataAsync();

            if (aggregatedData == null)
            {
                return NotFound();
            }

            return Ok(aggregatedData);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddSensorAndAlarmDataAsync()
        {
            string result = "Data inserted successfully!";

            try
            {
                string sensorDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Csv", "sensor_data.csv");
                string alarmDataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Csv", "alarm.csv");

                sensorDataFilePath = Path.GetFullPath(sensorDataFilePath);
                alarmDataFilePath = Path.GetFullPath(alarmDataFilePath);

                await dataService.InsertSensorDataBatchAsync(sensorDataFilePath);
                await dataService.InsertAlarmsBatchAsync(alarmDataFilePath);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            
            return Ok(result);
        }
    }
}
