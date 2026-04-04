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
            var userId = _httpContextAccessor.HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
