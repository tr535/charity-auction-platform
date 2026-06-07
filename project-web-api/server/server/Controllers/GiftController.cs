using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces;
using server.DTOs;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : ControllerBase
    {
        private readonly IGiftService _giftService;

        public GiftController(IGiftService giftService) => _giftService = giftService;

        // צפייה ברשימת המתנות - פתוח לכולם
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] string? category, [FromQuery] string? sortBy)
        {
            var response = await _giftService.GetAllGiftsAsync(name, category, null, null, sortBy);
            return Ok(response);
        }

        // חיפוש מתקדם למנהל
        [Authorize(Roles = "Manager")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin([FromQuery] string? name, [FromQuery] string? category, [FromQuery] string? donorName, [FromQuery] int? minPurchasers)
        {
            var response = await _giftService.GetAllGiftsAsync(name, category, donorName, minPurchasers, null);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _giftService.GetGiftByIdAsync(id);
            if (!response.Success) return NotFound(response);
            return Ok(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GiftDto giftDto)
        {
            var response = await _giftService.AddGiftAsync(giftDto);
            if (!response.Success) return BadRequest(response);
            return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GiftDto giftDto)
        {
            var response = await _giftService.UpdateGiftAsync(id, giftDto);
            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _giftService.RemoveGiftAsync(id);
            if (!response.Success) return BadRequest(response); // מחזיר BadRequest אם יש רכישות
            return Ok(response);
        }
    }
}