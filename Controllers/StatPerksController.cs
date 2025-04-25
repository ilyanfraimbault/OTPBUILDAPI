using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatPerksController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatPerks>>> GetStatPerks()
    {
        return await context.StatPerks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StatPerks>> GetStatPerks(int id)
    {
        var statPerks = await context.StatPerks.FindAsync(id);

        if (statPerks == null)
        {
            return NotFound();
        }

        return statPerks;
    }

    [HttpPost]
    public async Task<ActionResult<StatPerks>> PostStatPerks(StatPerks statPerk)
    {
        context.StatPerks.Add(statPerk);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStatPerks), new { id = statPerk.Id }, statPerk);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStatPerks(int id, StatPerks statPerk)
    {
        if (id != statPerk.Id)
        {
            return BadRequest();
        }

        context.Entry(statPerk).State = EntityState.Modified;
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatPerks(int id)
    {
        var statPerks = await context.StatPerks.FindAsync(id);
        if (statPerks == null)
        {
            return NotFound();
        }

        context.StatPerks.Remove(statPerks);
        await context.SaveChangesAsync();

        return NoContent();
    }
}