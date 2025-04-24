using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
    {
        return await context.Participants
            .Include(p => p.Game)
            .Include(p => p.Summoner)
            .Include(p => p.Perks)
            .ToListAsync();
    }

    [HttpGet("{gameId:long}/{puuid}")]
    public async Task<ActionResult<Participant>> GetParticipant(long gameId, string puuid)
    {
        var participant = await context.Participants
            .Include(p => p.Game)
            .Include(p => p.Summoner)
            .Include(p => p.Perks)
            .FirstOrDefaultAsync(p => p.GameId == gameId && p.SummonerPuuid == puuid);

        if (participant == null)
        {
            return NotFound();
        }

        return participant;
    }

    [HttpPost]
    public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
    {
        context.Participants.Add(participant);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetParticipant), new { gameId = participant.GameId, puuid = participant.SummonerPuuid }, participant);
    }

    [HttpPut("{gameId:long}/{puuid}")]
    public async Task<IActionResult> PutParticipant(long gameId, string puuid, Participant participant)
    {
        if (gameId != participant.GameId || puuid != participant.SummonerPuuid)
        {
            return BadRequest();
        }

        context.Entry(participant).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var exists = await context.Participants.AnyAsync(p => p.GameId == gameId && p.SummonerPuuid == puuid);
            if (!exists)
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{gameId:long}/{puuid}")]
    public async Task<IActionResult> DeleteParticipant(long gameId, string puuid)
    {
        var participant = await context.Participants.FindAsync(gameId, puuid);
        if (participant == null)
        {
            return NotFound();
        }

        context.Participants.Remove(participant);
        await context.SaveChangesAsync();

        return NoContent();
    }
}