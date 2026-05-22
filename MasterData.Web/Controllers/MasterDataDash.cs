using Microsoft.AspNetCore.Mvc;
using Domain.Services;

namespace MasterDataV2.Controllers
{
    public class MasterDataDash(DashboardService service) : Controller
    {
        public async Task<IActionResult> Index(DateTime? de, DateTime? ate)
        {
            var vm = await service.GetValuesDashboard(de, ate);
            return View(vm);
        }
    }
}
