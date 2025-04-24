using Microsoft.EntityFrameworkCore;
using OTPBUILDAPI.Models;

namespace OTPBUILDAPI.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Summoner> Summoners { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}