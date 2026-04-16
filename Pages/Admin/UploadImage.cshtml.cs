using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KresWebSitesi.Data;
using KresWebSitesi.Models;

namespace KresWebSitesi.Pages.Admin;

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

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Upload == null || Upload.Length == 0) return Page();

        // Dosya adını benzersiz yapalım
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Upload.FileName);
        var filePath = Path.Combine(_environment.WebRootPath, "uploads/gallery", fileName);

        // Klasöre kaydet
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await Upload.CopyToAsync(stream);
        }

        // Veritabanına kaydet
        _context.GalleryImages.Add(new GalleryImage
        {
            ImagePath = "/uploads/gallery/" + fileName,
            Description = Description ?? "" 
        });

        await _context.SaveChangesAsync();
        return RedirectToPage("/Index"); // Yükleme bitince ana sayfaya dön
    }
}