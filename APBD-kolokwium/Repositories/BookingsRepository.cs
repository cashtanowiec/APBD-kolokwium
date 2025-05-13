using APBD_kolokwium.DTO;
using APBD_kolokwium.Exceptions;
using APBD_kolokwium.Services;
using Microsoft.Data.SqlClient;

namespace APBD_kolokwium.Repositories;

public class BookingsRepository : IBookingsRepository
{
    private readonly String connectionString;

    public BookingsRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("Default");
    }
    
    public async Task<GetBookingInfoDTO> Get(int id)
    {
        String sql =
            @"select Booking.date, Guest.first_name, Guest.last_name, Guest.date_of_birth, Employee.first_name, Employee.last_name, Employee.employee_number, Attraction.name, Attraction.price, Booking_Attraction.amount
              from Booking 
                join Guest on Booking.guest_id = Guest.guest_id
                join Employee on Booking.employee_id = Employee.employee_id
                join Booking_Attraction on Booking.booking_id = Booking_Attraction.booking_id
                join Attraction on Booking_Attraction.attraction_id = Attraction.attraction_id
              where Booking.booking_id = @booking_id";

        await using SqlConnection connection = new SqlConnection(connectionString);
        await using SqlCommand command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@booking_id", id);
        await connection.OpenAsync();
        
        var reader = await command.ExecuteReaderAsync();

        GetBookingInfoDTO bookingInfoDTO = null;
        while (await reader.ReadAsync())
        {
            if (bookingInfoDTO == null)
            {
                bookingInfoDTO = new GetBookingInfoDTO()
                {
                    Date = reader.GetDateTime(0),
                    Guest = new GuestDTO()
                    {
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        DateOfBirth = reader.GetDateTime(3),
                    },
                    Employee = new EmployeeDTO()
                    {
                        FirstName = reader.GetString(4),
                        LastName = reader.GetString(5),
                        EmployeeNumber = reader.GetString(6)
                    },
                    Attractions = new List<GetAttractionDTO>()
                };
            }
            
            var attraction = new GetAttractionDTO()
            {
                Name = reader.GetString(7),
                Price = Convert.ToDouble(reader.GetDecimal(8)),
                Amount = reader.GetInt32(9)
            };
            bookingInfoDTO.Attractions.Add(attraction);
        }
        
        
        return bookingInfoDTO;
    }

    public async Task CheckIfBookingExists(SqlConnection sqlConnection, int bookingId)
    {
        String sql = "select 1 from Booking where booking_id = @booking_id";
        await using SqlCommand command = new SqlCommand(sql, sqlConnection);
        
        command.Parameters.AddWithValue("@booking_id", bookingId);
        var value = command.ExecuteScalarAsync();
        if (value == null)
        {
            throw new NotFoundException("Booking not found");
        }
    }

    public async Task CheckIfGuestExists(SqlConnection sqlConnection, int guestId)
    {
        String sql = "select 1 from Guest where guest_id = @guest_id";
        await using SqlCommand command = new SqlCommand(sql, sqlConnection);
        
        command.Parameters.AddWithValue("@guest_id", guestId);
        var value = command.ExecuteScalarAsync();
        if (value == null)
        {
            throw new NotFoundException("Guest not found");
        }
    }

    public async Task CheckIfEmployeeExists(SqlConnection connection, string employeeId)
    {
        String sql = "select 1 from Employee where employee_id = @employee_id";
        await using SqlCommand command = new SqlCommand(sql, connection);
        
        command.Parameters.AddWithValue("@employee_id", employeeId);
        var value = command.ExecuteScalarAsync();
        if (value == null)
        {
            throw new NotFoundException("Guest not found");
        }
    }

    public async Task CheckIfAttractionExists(SqlConnection connection, List<PostAttractionDTO> attractions)
    {
        foreach (PostAttractionDTO attraction in attractions)
        {
            String sql = "select 1 from Attraction where name = @name";
            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", attraction.Name);
            var value = command.ExecuteScalarAsync();
            if (value == null)
            {
                throw new NotFoundException("Attraction not found");
            }
        }
    }
    
    public async Task Add(SqlConnection connection, AddBookingDTO bookingDto)
    {

        foreach (PostAttractionDTO attraction in bookingDto.Attractions)
        {
            String sql = "select Attraction.attraction_id from Attraction where name = @name";
            await using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", attraction.Name);
            var value = command.ExecuteScalarAsync();
            if (value == null)
            {
                throw new NotFoundException("Attraction not found");
            }

            String insert = "insert into Booking_Attraction VALUES(@booking_id, @attraction_id, @amount)";
            command.CommandText = insert;
            command.Parameters.AddWithValue("@booking_id", bookingDto.BookingID);
            command.Parameters.AddWithValue("@attraction_id", value);
            command.Parameters.AddWithValue("@amount", attraction.Amount);

        }

        var list = bookingDto.Attractions;
    }
}