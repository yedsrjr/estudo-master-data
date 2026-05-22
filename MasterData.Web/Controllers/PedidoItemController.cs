using JJMasterData.Web.Extensions;
using Domain.Models.Enums;
using Domain.Models.ViewModels;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterDataV2.Controllers
{
    public class PedidoItemController(PedidoItemService service, LogOrderService logService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int codPedido)
        {
            var dataPanel = await service.SetupDataPanelAddItens(codPedido);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;
            ViewBag.CodPedido = codPedido;

            var vm = new PanelViewModel
            {
                DataPanel = dataPanel
            };

            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(int codPedido)
        {
            var dataPanel = await service.SetupDataPanelAddItens(codPedido);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;
            ViewBag.CodPedido = codPedido;

            var vm = await service.SaveItem(dataPanel);

            if (vm.ErrorAlert == null)
            {
                TempData["Mensagem"] = "Item adicionado com sucesso!";
                await logService.SaveLog(codPedido, "Novo item adicionado", (int)LogStatus.Insert);
                return RedirectToAction("Edit", "Pedidos", new { codPedido });
            }

            return View("Create", vm);
        }
    }
}
