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
    public async Task<ActionResult<Perks>> PostPerk(Perks perk)
    {
        context.Perks.Add(perk);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerk), new { id = perk.Id }, perk);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerk(int id, Perks perk)
    {
        if (id != perk.Id)
        {
            return BadRequest();
        }

        context.Entry(perk).State = EntityState.Modified;

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