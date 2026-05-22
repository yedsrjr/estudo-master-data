using JJMasterData.Core.UI.Components;
using JJMasterData.Web.Extensions;
using Domain.Models.ViewModels;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.Web.Controllers
{
    public class ProdutosController(ProdutoService service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await service.SetupFormView();

            if (result is IActionResult actionResult)
            {
                return actionResult;
            }

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        public async Task<IActionResult> GetProductDetails(string codProd)
        {
            var product = await service.SetupDataPanelView(codProd);

            var result = await product.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAdd(string codProd)
        {
            var dataPanel = await service.SetupDataPanelAdd(codProd);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            ViewBag.CodProd = codProd;

            var vm = new ProdutoViewModel
            {
                DataPanel = dataPanel
            };

            return View("Add", vm);
        }

        [HttpPost]
        public async Task<IActionResult> PostAdd(string codProd)
        {

            var dataPanel = await service.SetupDataPanelAdd(codProd);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            var vm = await service.SaveProduct(dataPanel);

            return View("Add", vm);
        }

    }
}
