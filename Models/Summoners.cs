namespace OTPBUILDAPI.Models;

using System.ComponentModel.DataAnnotations;

public class Summoner
{
    public string Id { get; set; }

    [Key]
    public string Puuid { get; set; }

    public string? Name { get; set; }
    public string? AccountId { get; set; }
    public int? ProfileIconId { get; set; }
    public long? RevisionDate { get; set; }
    public long? Level { get; set; }
    public string PlatformId { get; set; }
    public string? GameName { get; set; }
    public string? TagLine { get; set; }
}