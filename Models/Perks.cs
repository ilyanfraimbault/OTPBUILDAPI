using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTPBUILDAPI.Models;

public class Perks
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int StatPerksId { get; set; }

    [Required]
    public int PrimaryStyleId { get; set; }

    [Required]
    public int SecondaryStyleId { get; set; }

    [ForeignKey(nameof(StatPerksId))]
    public StatPerks StatPerks { get; set; }

    [ForeignKey(nameof(PrimaryStyleId))]
    public PerksStyle PrimaryStyle { get; set; }

    [ForeignKey(nameof(SecondaryStyleId))]
    public PerksStyle SecondaryStyle { get; set; }
}