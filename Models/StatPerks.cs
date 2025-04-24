using System.ComponentModel.DataAnnotations;

namespace OTPBUILDAPI.Models;

public class StatPerks
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Defense { get; set; }

    [Required]
    public int Flex { get; set; }

    [Required]
    public int Offense { get; set; }
}