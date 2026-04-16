using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KresWebSitesi.Data;
using KresWebSitesi.Models;

namespace KresWebSitesi.Pages.Admin;

[Authorize]
public class UploadImageModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public UploadImageModel(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [BindProperty]
    public IFormFile Upload { get; set; } = null!;
    
    [BindProperty]
    public string Description { get; set; } = string.Empty;

    // Sayfada listelenecek fotoğrafları tutan liste
    public IList<GalleryImage> Images { get; set; } = default!;

    public async Task OnGetAsync()
    {
        // Sayfa açıldığında veritabanındaki tüm fotoğrafları getir
        Images = await _context.GalleryImages.OrderByDescending(g => g.UploadDate).ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Upload == null || Upload.Length == 0) return RedirectToPage();

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Upload.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "uploads/gallery", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await Upload.CopyToAsync(stream);
        }

        _context.GalleryImages.Add(new GalleryImage
        {
            ImagePath = "/uploads/gallery/" + fileName,
            Description = Description ?? ""
        });

        await _context.SaveChangesAsync();
        return RedirectToPage(); // İşlem bitince aynı sayfayı yenile
    }

    // --- YENİ EKLENEN SİLME METODU ---
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var image = await _context.GalleryImages.FindAsync(id);
        if (image != null)
        {
            // 1. Fiziksel dosyayı klasörden sil
            var filePath = Path.Combine(_environment.WebRootPath, image.ImagePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // 2. Kaydı veritabanından sil
            _context.GalleryImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage(); // Sayfayı yenile
    }
}