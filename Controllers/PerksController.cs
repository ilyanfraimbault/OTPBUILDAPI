using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PerksController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Perks>>> GetPerks()
    {
        return await context.Perks.Include(p => p.StatPerks)
            .Include(p => p.PrimaryStyle)
            .Include(p => p.SecondaryStyle)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Perks>> GetPerk(int id)
    {
        var perk = await context.Perks.Include(p => p.StatPerks)
            .Include(p => p.PrimaryStyle)
            .Include(p => p.SecondaryStyle)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (perk == null)
        {
            return NotFound();
        }

        return perk;
    }

    [HttpPost]
    public async Task<ActionResult<Perks>> PostPerk(Perks perks)
    {
        context.Perks.Add(perks);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerk), new { id = perks.Id }, perks);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerk(int id, Perks perks)
    {
        if (id != perks.Id)
        {
            return BadRequest();
        }

        context.Entry(perks).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Perks.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerk(int id)
    {
        var perk = await context.Perks.FindAsync(id);
        if (perk == null)
        {
            return NotFound();
        }

        context.Perks.Remove(perk);
        await context.SaveChangesAsync();

        return NoContent();
    }
}