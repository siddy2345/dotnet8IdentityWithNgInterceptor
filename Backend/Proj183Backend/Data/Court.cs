namespace Proj183Backend.Data;

public class Court
{
    public int CourtId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


    public int UserId { get; set; }

    public User? User { get; set; }

    public int AddressId { get; set; }

    public Address? Address { get; set; }
}
