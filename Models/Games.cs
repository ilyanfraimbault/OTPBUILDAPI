using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OTPBUILDAPI.Models;

public class Game
{
    [Key]
    public long GameId { get; set; }

    [Required]
    public int GameDuration { get; set; }

    [Required]
    public long GameStartTimestamp { get; set; }

    [Required]
    [StringLength(50)]
    public string GameVersion { get; set; }

    [Required]
    [StringLength(50)]
    public string GameType { get; set; }

    [Required]
    [StringLength(50)]
    public string MatchId { get; set; }

    [Required]
    [StringLength(10)]
    public string PlatformId { get; set; }

    [Required]
    public int Winner { get; set; }
}