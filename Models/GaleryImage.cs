namespace KresWebSitesi.Models;

public class GalleryImage
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty; // Fotoğrafın sunucudaki yolu
    public string Description { get; set; } = string.Empty; // Fotoğraf alt metni
    public DateTime UploadDate { get; set; } = DateTime.Now;
}