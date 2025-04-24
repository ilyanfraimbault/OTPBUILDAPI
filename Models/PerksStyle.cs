namespace OTPBUILDAPI.Models;

using System.ComponentModel.DataAnnotations;

public class PerksStyle
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Description { get; set; }

    [Required]
    public int Style { get; set; }

    [Required]
    public int StyleSelection1 { get; set; }

    [Required]
    public int StyleSelection2 { get; set; }

    public int? StyleSelection3 { get; set; }

    public int? StyleSelection4 { get; set; }
}