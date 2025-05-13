using APBD_kolokwium.DTO;

namespace APBD_kolokwium.Services;

public interface IBookingsService
{
    Task<GetBookingInfoDTO> Get(int id);
    Task Add(AddBookingDTO bookingDto);
}