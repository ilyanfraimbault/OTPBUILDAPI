using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Data;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PerksStylesController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPerksStyles()
    {
        try
        {
            var perksStyles = await context.PerksStyles.ToListAsync();

            if (!perksStyles.Any())
            {
                return NotFound(new { Message = "Aucun style de perks trouvé." });
            }

            return Ok(perksStyles);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur interne : {ex.Message}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PerksStyle>> GetPerksStyle(int id)
    {
        var perksStyle = await context.PerksStyles.FindAsync(id);
        if (perksStyle == null)
        {
            return NotFound();
        }
        return perksStyle;
    }

    [HttpPost]
    public async Task<ActionResult<PerksStyle>> PostPerksStyle(PerksStyle perksStyle)
    {
        context.PerksStyles.Add(perksStyle);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPerksStyle), new { id = perksStyle.Id }, perksStyle);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePerksStyle(int id, PerksStyle perksStyle)
    {
        if (id != perksStyle.Id)
        {
            return BadRequest();
        }

        context.Entry(perksStyle).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.PerksStyles.Any(e => e.Id == id))
                return NotFound();

            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePerksStyle(int id)
    {
        var perksStyle = await context.PerksStyles.FindAsync(id);
        if (perksStyle == null)
        {
            return NotFound();
        }

        context.PerksStyles.Remove(perksStyle);
        await context.SaveChangesAsync();

        return NoContent();
    }
}