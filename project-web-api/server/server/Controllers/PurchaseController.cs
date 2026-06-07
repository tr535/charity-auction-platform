using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces;
using server.Models.DTO;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _purchaseService.GetAllPurchasesAsync();
            return Ok(response);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Post([FromBody] PurchaseRequestDTO purchaseRequest)
        {
            if (purchaseRequest == null) return BadRequest("Invalid request");
            var response = await _purchaseService.AddPurchaseAsync(purchaseRequest);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("confirm/{userId}")]
        public async Task<IActionResult> ConfirmCart(int userId)
        {
            var response = await _purchaseService.ConfirmCartAsync(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("cart/{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var response = await _purchaseService.GetUserCartAsync(userId);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _purchaseService.RemovePurchaseAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}