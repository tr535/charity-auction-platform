using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.BLL.Interfaces;
using server.Models.DTO;

[Authorize(Roles = "Manager")]
[Route("api/[controller]")]
[ApiController]
public class DonorController : ControllerBase
{
    private readonly IDonorService _donorService;

    public DonorController(IDonorService donorService) => _donorService = donorService;

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string? name, [FromQuery] string? email)
    {
        var response = await _donorService.GetAllDonorsAsync(name, email);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await _donorService.GetDonorByIdAsync(id);
        return response.Success ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DonorDTO donorDTO)
    {
        var response = await _donorService.AddDonorAsync(donorDTO);
        return response.Success ? CreatedAtAction(nameof(Get), new { id = response.Data.Id }, response) : BadRequest(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] DonorDTO donorDTO)
    {
        var response = await _donorService.UpdateDonorAsync(id, donorDTO);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _donorService.RemoveDonorAsync(id);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}