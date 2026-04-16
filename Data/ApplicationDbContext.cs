using Microsoft.EntityFrameworkCore;
using KresWebSitesi.Models;

namespace KresWebSitesi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<GalleryImage> GalleryImages { get; set; }
}