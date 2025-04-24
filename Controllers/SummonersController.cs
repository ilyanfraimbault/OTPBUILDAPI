using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SummonersController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Summoner>>> GetSummoners(int page = 1, int pageSize = 50)
    {
        var summoners = await context.Summoners
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(summoners);
    }

    [HttpGet("{puuid}")]
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