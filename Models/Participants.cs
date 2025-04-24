using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTPBUILDAPI.Models;

[Table("Participants")]
public class Participant
{
    [Key, Column(Order = 0)]
    public long GameId { get; set; }

    [Key, Column(Order = 1)]
    [StringLength(78)]
    public string SummonerPuuid { get; set; }

    public int Champion { get; set; }
    public int TeamId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }

    public int item0 { get; set; }
    public int item1 { get; set; }
    public int item2 { get; set; }
    public int item3 { get; set; }
    public int item4 { get; set; }
    public int item5 { get; set; }
    public int item6 { get; set; }

    public int spellCast1 { get; set; }
    public int spellCast2 { get; set; }
    public int spellCast3 { get; set; }
    public int spellCast4 { get; set; }

    public int SummonerSpell1 { get; set; }
    public int SummonerSpell2 { get; set; }
    public int PerksId { get; set; }

    [Required, StringLength(10)]
    public string TeamPosition { get; set; }

    [ForeignKey(nameof(GameId))]
    public Game Game { get; set; }

    [ForeignKey(nameof(SummonerPuuid))]
    public Summoner Summoner { get; set; }

    [ForeignKey(nameof(PerksId))]
    public Perks Perks { get; set; }
}