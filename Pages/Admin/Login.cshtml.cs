using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KresWebSitesi.Pages.Admin;

public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    public LoginModel(IConfiguration configuration) => _configuration = configuration;

    [BindProperty] public string Username { get; set; } = string.Empty;
    [BindProperty] public string Password { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public async Task<IActionResult> OnPostAsync()
    {
        var adminUser = _configuration.GetSection("AdminUser");
        if (Username == adminUser["Username"] && Password == adminUser["Password"])
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, Username) };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return RedirectToPage("/Admin/UploadImage");
        }
        Message = "Geçersiz kullanıcı adı veya şifre!";
        return Page();
    }
}