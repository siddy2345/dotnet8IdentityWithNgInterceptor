using System.ComponentModel.DataAnnotations;

namespace Proj183Backend.Controllers.Models;

public class CourtModel
{
    public int CourtId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
    [MinLength(1, ErrorMessage = "Title must be set.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
    [MinLength(1, ErrorMessage = "Street must be set.")]
    public string Street { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "Street number cannot be longer than 100.")]
    public string Number { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "City cannot be longer than 100 characters.")]
    [MinLength(1, ErrorMessage = "City must be set.")]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(100, ErrorMessage = "Canton cannot be longer than 100 characters.")]
    [MinLength(1, ErrorMessage = "Canton must be set.")]
    public string Canton { get; set; } = string.Empty;

    [MaxLength(9999, ErrorMessage = "PLZ cannot be greater than 9999.")]
    public int PLZ { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }
}
