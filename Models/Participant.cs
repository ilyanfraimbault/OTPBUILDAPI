using System.ComponentModel.DataAnnotations.Schema;

namespace OTPBUILDAPI.Models;

public class Participant
{
    public long GameId { get; set; }
    public string SummonerPuuid { get; set; }
    public int Champion { get; set; }
    public int TeamId { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int Item0 { get; set; }
    public int Item1 { get; set; }
    public int Item2 { get; set; }
    public int Item3 { get; set; }
    public int Item4 { get; set; }
    public int Item5 { get; set; }
    public int Item6 { get; set; }
    public int SpellCast1 { get; set; }
    public int SpellCast2 { get; set; }
    public int SpellCast3 { get; set; }
    public int SpellCast4 { get; set; }
    public int SummonerSpell1 { get; set; }
    public int SummonerSpell2 { get; set; }
    public int PerksId { get; set; }
    public string TeamPosition { get; set; }

    public Game Game { get; set; }

    [ForeignKey(nameof(PerksId))]
    public Perks Perks { get; set; }

    [ForeignKey(nameof(SummonerPuuid))]
    public Summoner Summoner { get; set; }
}