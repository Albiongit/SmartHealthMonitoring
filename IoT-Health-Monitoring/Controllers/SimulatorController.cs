using IoT_Health_Monitoring.Services;
using Microsoft.AspNetCore.Mvc;


namespace IoT_Health_Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorController : ControllerBase
    {
        private readonly SimulatorService simulatorService;

        public SimulatorController(SimulatorService simulatorService)
        {
            this.simulatorService = simulatorService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> GenerateDataAsync(int nrOfRows, CancellationToken cancellationToken)
        {
            string response = await simulatorService.ProduceMessageAsync(nrOfRows, cancellationToken);

            return Ok(response);
        }
    }
}
