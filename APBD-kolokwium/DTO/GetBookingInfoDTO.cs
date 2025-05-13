using System.ComponentModel.DataAnnotations;

namespace APBD_kolokwium.DTO;

public class GetBookingInfoDTO
{
    [Required]
    public DateTime Date { get; set; }
    [Required] 
    public GuestDTO Guest { get; set; }
    [Required]
    public EmployeeDTO Employee { get; set; }
    [Required]
    public List<GetAttractionDTO> Attractions { get; set; }
}

public class GuestDTO
{
    [Required]
    public String FirstName { get; set; }
    [Required]
    public String LastName { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
}

public class EmployeeDTO
{
    [Required]
    public String FirstName { get; set; }
    [Required]
    public String LastName { get; set; }
    [Required]
    public String EmployeeNumber { get; set; }
}

public class GetAttractionDTO
{
    [Required]
    public String Name { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public int Amount { get; set; }
}
