using Microsoft.AspNetCore.Identity;

namespace Proj183Backend.Data;

public class User : IdentityUser<int>
{
    public ICollection<Court> Courts { get; set; } = new List<Court>();
}
