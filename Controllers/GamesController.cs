using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetGames(int page = 1, int pageSize = 20)
    {
        try
        {
            var totalGames = await context.Games.CountAsync();
            var gameIds = await context.Games
                .OrderBy(g => g.GameId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => g.GameId) // Projection pour ne retourner que les IDs
                .ToListAsync();

            if (!gameIds.Any())
            {
                return NotFound(new { Message = "Aucun jeu trouvé." });
            }

            var result = new
            {
                TotalCount = totalGames,
                Page = page,
                PageSize = pageSize,
                GameIds = gameIds
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Game>> GetGame(long id)
    {
        var game = await context.Games
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.PrimaryStyle)
            .Include(game => game.Participants)
            .ThenInclude(participant => participant.Perks).ThenInclude(perks => perks.SecondaryStyle)
            .Include(game => game.Participants).ThenInclude(participant => participant.Perks)
            .ThenInclude(perks => perks.StatPerks)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Summoner)
            .FirstOrDefaultAsync(g => g.GameId == id);

        if (game == null)
        {
            return NotFound(new { Message = "No game found with this ID." });
        }

        return Ok(game);
    }

    [HttpPost]
    public async Task<ActionResult<Game>> PostGame(Game game)
    {
        context.Games.Add(game);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, game);
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> PutGame(long id, Game game)
    {
        if (id != game.GameId) return BadRequest();

        context.Entry(game).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Games.Any(g => g.GameId == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteGame(long id)
    {
        var game = await context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        context.Games.Remove(game);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("by-puuid/{puuid}")]
    public async Task<ActionResult<IEnumerable<Game>>> GetGamesByPuuid(
        string puuid,
        int page = 1,
        int pageSize = 20)
    {
        if (page < 1 || pageSize < 1)
            return BadRequest(new { Message = "Page et pageSize doivent être supérieurs à 0." });

        var gamesQuery = context.Games
            .Where(g => g.Participants.Any(p => p.SummonerPuuid == puuid))
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.PrimaryStyle)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.SecondaryStyle)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Perks)
            .ThenInclude(perks => perks.StatPerks)
            .Include(g => g.Participants)
            .ThenInclude(p => p.Summoner)
            .OrderByDescending(g => g.GameStartTimestamp);

        var totalGames = await gamesQuery.CountAsync();

        var games = await gamesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            Page = page,
            PageSize = pageSize,
            TotalGames = totalGames,
            TotalPages = (int)Math.Ceiling(totalGames / (double)pageSize),
            Games = games
        });
    }
}