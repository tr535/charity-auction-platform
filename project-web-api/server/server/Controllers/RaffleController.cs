using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces;
using server.Models.DTO;

namespace server.Controllers
{
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly IRaffleService _raffleService;

        public RaffleController(IRaffleService raffleService)
        {
            _raffleService = raffleService;
        }

        [HttpPost("draw/{giftId}")]
        public async Task<IActionResult> Draw(int giftId)
        {
            var response = await _raffleService.ExecuteDrawAsync(giftId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("total-income")]
        public async Task<IActionResult> GetIncome()
        {
            var response = await _raffleService.GetTotalIncomeAsync();
            return Ok(response);
        }

        [HttpGet("winners-report")]
        public async Task<IActionResult> GetWinners()
        {
            var response = await _raffleService.GetWinnersReportAsync();
            return Ok(response);
        }
    }
}