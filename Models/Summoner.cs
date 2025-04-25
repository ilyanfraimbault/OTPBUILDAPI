namespace OTPBUILDAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Summoner
{
    [Required]
    [StringLength(63)]
    public string Id { get; init; }

    [Key]
    [Required]
    [StringLength(78)]
    public string Puuid { get; init; }

    [StringLength(50)] public string? Name { get; init; }

    [StringLength(56)] public string? AccountId { get; init; }

    public int? ProfileIconId { get; init; }
    public long? RevisionDate { get; init; }
    public long? Level { get; init; }

    [Required]
    [StringLength(10)]
    public string PlatformId { get; init; }

    [StringLength(50)] public string? GameName { get; init; }

    [StringLength(5)] public string? TagLine { get; init; }
}