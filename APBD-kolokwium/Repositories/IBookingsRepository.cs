using APBD_kolokwium.DTO;
using Microsoft.Data.SqlClient;

namespace APBD_kolokwium.Repositories;

public interface IBookingsRepository
{
    Task<GetBookingInfoDTO> Get(int id);
    Task CheckIfBookingExists(SqlConnection sqlConnection, int bookingId);
    Task CheckIfGuestExists(SqlConnection sqlConnection, int guestId);
    Task CheckIfEmployeeExists(SqlConnection sqlConnection, String employeeId);
    Task CheckIfAttractionExists(SqlConnection connection, List<PostAttractionDTO> attractions);
    Task Add(SqlConnection sqlConnection, AddBookingDTO bookingDto);
}