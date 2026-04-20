using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BettingSystem.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin") return RedirectToPage("/Index");

            return Page();
        }
    }
}
