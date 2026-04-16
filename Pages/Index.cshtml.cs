using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KresWebSitesi.Data;
using KresWebSitesi.Models;

namespace KresWebSitesi.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    // Veritabanı bağlantısını sayfaya dahil ediyoruz (Dependency Injection)
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // Arayüze göndereceğimiz fotoğraf listesi
    public IList<GalleryImage> GalleryImages { get; set; } = default!;

    public async Task OnGetAsync()
    {
        // Sayfa açıldığında veritabanındaki fotoğrafları en yeniden en eskiye sıralayarak alıyoruz
        GalleryImages = await _context.GalleryImages
            .OrderByDescending(g => g.UploadDate)
            .ToListAsync();
    }
}