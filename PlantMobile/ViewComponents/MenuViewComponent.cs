using Microsoft.AspNetCore.Mvc;
using PlantMobile.Infrastructure;
using PlantMobile.Models.Menu;

namespace PlantMobile.ViewComponents;

/// <summary>
/// Renders the navigation menu for the signed-in user.
/// Replaces the three nested child actions of the legacy DrawMenu controller.
/// </summary>
public sealed class MenuViewComponent(IMenuService menuService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var session = HttpContext.Session;
        if (session.GetString("UserSession") != "Authenticated"
            || !int.TryParse(session.GetString("UserId"), out var userId))
        {
            return Content(string.Empty);
        }

        // The application has a single Arabic interface, so the Arabic titles are used.
        var groups = await menuService.GetAsync(userId, isArabic: true, HttpContext.RequestAborted);
        return View(MenuViewModel.Create(groups, HttpContext.Request));
    }
}
