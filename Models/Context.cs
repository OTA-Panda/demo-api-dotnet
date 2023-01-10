using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Models;

public class DemoContext : DbContext
{
  public DemoContext( DbContextOptions<DemoContext> options ) : base ( options )
  {
  }

  public DbSet<DemoItem> DemoItems { get; set; } = null!;
}