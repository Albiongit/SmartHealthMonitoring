using IoT_Health_Monitoring.Models;
using IoT_Health_Monitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoT_Health_Monitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly DataService _service;

        public DataController(DataService service)
        {
            _service = service;
        }

        [HttpGet("{sensorNodeId}")]
        public async Task<ActionResult<DataModel>> GetAggregatedData(Guid sensorNodeId)
        {
            var aggregatedData = await _service.GetAggregatedSensorData(sensorNodeId);

            if (aggregatedData == null)
            {
                return NotFound();
            }

            return Ok(aggregatedData);
        }
    }
}
