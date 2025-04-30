using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParticipantsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet("{gameId:long}/{puuid}")]
    public async Task<ActionResult<Participant>> GetParticipant(long gameId, string puuid)
    {
        var participant = await context.Participants
            .Include(p => p.Game)
            .Include(p => p.Summoner)
            .Include(p => p.Perks)
            .ThenInclude(p => p.PrimaryStyle)
            .Include(p => p.Perks)
            .ThenInclude(p => p.SecondaryStyle)
            .Include(p => p.Perks)
            .ThenInclude(p => p.StatPerks)
            .FirstOrDefaultAsync(p => p.GameId == gameId && p.SummonerPuuid == puuid);

        if (participant == null)
        {
            return NotFound();
        }

        return participant;
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