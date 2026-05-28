using JJMasterData.Web.Extensions;
using Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using MasterData.Domain.Services;

namespace MasterData.Web.Controllers
{
    public class PriceCustomerController(PriceCustomerService service) : Controller
    {
        // GET: PriceCustomer
        public async Task<IActionResult> Index()
        {
            var result = await service.SetupFormView();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        // GET: PriceCustomer/Details/5
        public async Task<IActionResult> Details(int customerId)
        {
            var data = await service.SetupDataPanelDetails(customerId);

            var result = await data.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        // GET: PriceCustomer/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var dataPanel = await service.SetupDataPanelCreate();

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;
            ViewBag.CustomerId = 0;

            var vm = new PanelViewModel
            {
                DataPanel = dataPanel
            };

            return View("Create", vm);
        }

        // POST: PriceCustomer/Create
        [HttpPost]
        public async Task<IActionResult> Create(int customerId)
        {
            var dataPanel = await service.SetupDataPanelCreate();

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;
            ViewBag.CustomerId = customerId;

            var vm = await service.SavePrice(dataPanel);

            if (vm.ErrorAlert == null)
            {
                TempData["Mensagem"] = "Registro salvo com sucesso!";
                return RedirectToAction("Create", new { customerId });
            }

            return View("Create", vm);
        }
    }
}
