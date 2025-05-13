using System.ComponentModel.DataAnnotations;

namespace APBD_kolokwium.DTO;

public class AddBookingDTO
{
    [Required]
    public int BookingID { get; set; }
    [Required]
    public int GuestID { get; set; }
    [Required]
    public String EmployeeNumber { get; set; }
    [Required]
    public List<PostAttractionDTO> Attractions { get; set; }
}

public class PostAttractionDTO
{
    [Required]
    public String Name { get; set; }
    [Required]
    public int Amount { get; set; }
}