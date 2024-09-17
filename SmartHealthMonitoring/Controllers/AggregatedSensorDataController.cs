using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Models;
using SmartHealthMonitoring.Services;

namespace SmartHealthMonitoring.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregatedSensorDataController : ControllerBase
    {
        private readonly AggregatedSensorDataService _service;

        public AggregatedSensorDataController(AggregatedSensorDataService service)
        {
            _service = service;
        }

        [HttpGet("{sensorNodeId}")]
        public async Task<ActionResult<AggregatedSensorDataModel>> GetAggregatedData(Guid sensorNodeId)
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
