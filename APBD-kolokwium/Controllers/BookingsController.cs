using APBD_kolokwium.DTO;
using APBD_kolokwium.Exceptions;
using APBD_kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_kolokwium.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingsService _bookingsService;
    public BookingsController(IBookingsService bookingsService)
    {
        _bookingsService = bookingsService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var data = await _bookingsService.Get(id);
            return Ok(data);
        }
        catch (NotFoundException exc)
        {
            return NotFound(exc.Message);
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }

    [HttpPost()]
    public async Task<IActionResult> Add(AddBookingDTO bookingDto)
    {
        try
        {
            await _bookingsService.Add(bookingDto);
            return Ok();
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }
}