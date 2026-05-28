using JJMasterData.Web.Extensions;
using Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MasterData.Domain.Services;

namespace MasterData.Web.Controllers
{
    public class ClientesController(ClienteService service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await service.SetupFormView();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        public async Task<IActionResult> Details(int codClient)
        {
            var client = await service.SetupDataPanelView(codClient);

            var result = await client.GetResultAsync();
            
            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAdd(int? codClient)
        {
            var dataPanel = await service.SetupDataPanelAdd(codClient);

            var result = await dataPanel.GetResultAsync();
            
            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            ViewBag.CodClient = codClient;

            var vm = new ClienteViewModel
            {
                DataPanel = dataPanel
            };

            return View("Add", vm);
        }

        [HttpPost]
        public async Task<IActionResult> PostAdd(int? codClient)
        {

            var dataPanel = await service.SetupDataPanelAdd(codClient);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            var vm = await service.SaveClient(dataPanel);

            return View("Add", vm);
        }
    }
}
