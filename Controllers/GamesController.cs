using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GamesController(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<IActionResult> GetGames(int page = 1, int pageSize = 50)
    {
        try
        {
            var totalGames = await _context.Games.CountAsync();
            var games = await _context.Games
                .Include(g => g.Participants)
                .ThenInclude(p => p.Perks)
                .ThenInclude(perks => perks.PrimaryStyle)
                .Include(g => g.Participants)
                .ThenInclude(p => p.Perks)
                .ThenInclude(perks => perks.SecondaryStyle)
                .Include(g => g.Participants)
                .ThenInclude(p => p.Summoner)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (!games.Any())
            {
                return NotFound(new { Message = "Aucun jeu trouvé." });
            }

            var result = new
            {
                TotalCount = totalGames,
                Page = page,
                PageSize = pageSize,
                Games = games.Select(game => new
                {
                    game.GameId,
                    game.GameDuration,
                    game.GameStartTimestamp,
                    game.GameVersion,
                    game.GameType,
                    game.MatchId,
                    game.PlatformId,
                    game.Winner,
                    Participants = game.Participants.Select(participant => new
                    {
                        participant.SummonerPuuid,
                        participant.Champion,
                        participant.TeamId,
                        participant.Kills,
                        participant.Deaths,
                        participant.Assists,
                        Perks = new
                        {
                            participant.Perks.StatPerks,
                            participant.Perks.PrimaryStyle,
                            participant.Perks.SecondaryStyle
                        },
                        Summoner = new
                        {
                            participant.Summoner.AccountId,
                            participant.Summoner.GameName,
                            participant.Summoner.TagLine,
                            participant.Summoner.Id,
                            participant.Summoner.Level,
                            participant.Summoner.Name,
                            participant.Summoner.Puuid,
                            participant.Summoner.ProfileIconId,
                            participant.Summoner.RevisionDate,
                            participant.Summoner.PlatformId
                        }
                    }).ToList()
                })
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur interne : {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Game>> GetGame(long id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        return game;
    }

    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutGame(long id, Game game)
    {
        if (id != game.GameId)
        {
            return BadRequest();
        }

        _context.Entry(game).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Games.Any(g => g.GameId == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteGame(long id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        _context.Games.Remove(game);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id:long}/details")]
    public async Task<IActionResult> GetGameDetails(long id)
    {
        Console.WriteLine($"Attempting to retrieve game with ID: {id}");

        var game = await _context.Games
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.PrimaryStyle)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.SecondaryStyle)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Summoner)
            .FirstOrDefaultAsync(g => g.GameId == id);


        if (game == null)
        {
            return NotFound(new { Message = "No game found with this ID." });
        }

        return Ok(game);
    }
}