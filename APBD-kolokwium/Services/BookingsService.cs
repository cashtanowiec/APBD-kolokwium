using System.Data.Common;
using APBD_kolokwium.DTO;
using APBD_kolokwium.Exceptions;
using APBD_kolokwium.Repositories;
using Microsoft.Data.SqlClient;

namespace APBD_kolokwium.Services;

public class BookingsService : IBookingsService
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly String connectionString;
    public BookingsService(IBookingsRepository bookingsRepository, IConfiguration configuration)
    {
        _bookingsRepository = bookingsRepository;
        connectionString = configuration.GetConnectionString("Default");
    }

    public async Task<GetBookingInfoDTO> Get(int id)
    {
        var data = await _bookingsRepository.Get(id);
        if (data == null) 
            throw new NotFoundException("Booking not found");
        
        return data;
    }

    public async Task Add(AddBookingDTO bookingDto)
    {
        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();

        DbTransaction transaction = connection.BeginTransaction();
        command.Transaction = transaction as SqlTransaction;

        try
        {
            await _bookingsRepository.CheckIfBookingExists(connection, bookingDto.BookingID);
            await _bookingsRepository.CheckIfGuestExists(connection, bookingDto.GuestID);
            await _bookingsRepository.CheckIfEmployeeExists(connection, bookingDto.EmployeeNumber);
            await _bookingsRepository.CheckIfAttractionExists(connection, bookingDto.Attractions);
            await _bookingsRepository.Add(connection, bookingDto);

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
    
    
}
