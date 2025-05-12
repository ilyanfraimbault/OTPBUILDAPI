using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SummonersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("search")]
    public async Task<IActionResult> SearchSummoners([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query is required");

        IQueryable<Summoner> summonersQuery;

        if (query.Contains("#"))
        {
            var parts = query.Split('#', 2);
            var gameName = parts[0];
            var tagLine = parts.Length > 1 ? parts[1] : string.Empty;

            summonersQuery = context.Summoners
                .Where(x => x.GameName != null
                            && x.TagLine != null
                            && x.GameName.Equals(gameName, StringComparison.CurrentCultureIgnoreCase)
                            && (string.IsNullOrEmpty(tagLine) || x.TagLine.ToLower().StartsWith(tagLine.ToLower())));
        }
        else
        {
            summonersQuery = context.Summoners
                .Where(x => x.GameName != null && x.GameName.ToLower().Contains(query.ToLower()));
        }

        var results = await summonersQuery
            .OrderBy(x => x.GameName)
            .Take(6)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Puuid,
                s.GameName,
                s.TagLine,
                s.Level,
                s.PlatformId,
                s.ProfileIconId,
                s.RevisionDate,
                s.AccountId
            })
            .ToListAsync();

        return Ok(results);
    }

    [HttpGet("by-riot-id/{gameName}/{tagLine}")]
    public async Task<IActionResult> GetSummonerByGameNameAndTagLine(string gameName, string tagLine)
    {
        if (string.IsNullOrWhiteSpace(gameName) || string.IsNullOrWhiteSpace(tagLine))
        {
            return BadRequest(new { Message = "GameName and TagLine are required." });
        }

        var summoner = await context.Summoners
            .FirstOrDefaultAsync(s => s.GameName == gameName && s.TagLine == tagLine);

        if (summoner == null)
        {
            return NotFound(new { Message = "No summoner found." });
        }

        return Ok(summoner);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Summoner>>> GetSummoners(int page = 1, int pageSize = 50)
    {
        var summoners = await context.Summoners
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(summoners);
    }

    [HttpGet("by-puuid/{puuid}")]
    public async Task<ActionResult<Summoner>> GetSummoner(string puuid)
    {
        var summoner = await context.Summoners.FindAsync(puuid);

        if (summoner == null)
        {
            return NotFound();
        }

        return summoner;
    }

    [HttpPost]
    public async Task<ActionResult<Summoner>> PostSummoner(Summoner summoner)
    {
        context.Summoners.Add(summoner);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSummoner), new { puuid = summoner.Puuid }, summoner);
    }

    [HttpPut("{puuid}")]
    public async Task<IActionResult> PutSummoner(string puuid, Summoner summoner)
    {
        if (puuid != summoner.Puuid)
        {
            return BadRequest();
        }

        context.Entry(summoner).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{puuid}")]
    public async Task<IActionResult> DeleteSummoner(string puuid)
    {
        var summoner = await context.Summoners.FindAsync(puuid);
        if (summoner == null)
        {
            return NotFound();
        }

        context.Summoners.Remove(summoner);
        await context.SaveChangesAsync();

        return NoContent();
    }
}