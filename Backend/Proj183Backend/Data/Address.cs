namespace Proj183Backend.Data;

public class Address
{
    public int AddressId { get; set; }

    public string Street { get; set; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Canton { get; set; } = string.Empty;

    public int PLZ { get; set; }

    public ICollection<Court> Courts { get; set; } = new List<Court>();
}
