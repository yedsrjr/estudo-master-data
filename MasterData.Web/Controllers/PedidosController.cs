using Domain.Models.Enums;
using Domain.Models.ViewModels;
using Domain.Services;
using JJMasterData.Web.Extensions;
using MasterData.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterData.Web.Controllers
{
    public class PedidosController(PedidoService service, PedidoItemService itemService,
        LogOrderService logService, OrderValidate validate) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var result = await service.SetupFormView();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int codPedido)
        {
            var dataPanel = await service.SetupDataPanelView(codPedido);
            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            var formViewItem = await itemService.SetupListView(codPedido);
            var resultItem = await formViewItem.GetResultAsync();

            if (resultItem is IActionResult actionResultItem)
                return actionResultItem;

            var formViewLog = await logService.SetupFormView(codPedido);
            var resultLog = await formViewLog.GetResultAsync();

            if (resultLog is IActionResult actionResultLog)
                return actionResultLog;

            ViewBag.ContentHeader = result.HtmlContent;
            ViewBag.ContentItem = resultItem.HtmlContent;
            ViewBag.ContentLog = resultLog.HtmlContent;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? codPedido)
        {
            var dataPanel = await service.SetupDataPanelPedido(codPedido);
            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;
           
            var formViewItem = await itemService.SetupFormViewItem(codPedido);
            var resultItem = await formViewItem.GetResultAsync();

            if (resultItem is IActionResult actionResultItem)
                return actionResultItem;

            var formViewLog = await logService.SetupFormView(codPedido);
            var resultLog = await formViewLog.GetResultAsync();

            if (resultLog is IActionResult actionResultLog)
                return actionResultLog;

            ViewBag.ContentHeader = result.HtmlContent;
            ViewBag.ContentItem = resultItem.HtmlContent;
            ViewBag.ContentLog = resultLog.HtmlContent;

            var vm = new PedidoViewModel
            {
                DataPanel = dataPanel,
                CodPedido = codPedido ?? 0
            };

            return View("Edit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int codPedido, string submitAction)
        {
            var dataPanel = await service.SetupDataPanelPedido(codPedido);
            var result = await dataPanel.GetResultAsync();
            if (result is IActionResult actionResult)
                return actionResult;

            var formViewItem = await itemService.SetupFormViewItem(codPedido);
            var resultItem = await formViewItem.GetResultAsync();
            if (resultItem is IActionResult actionResultItem)
                return actionResultItem;

            var formViewLog = await logService.SetupFormView(codPedido);
            var resultLog = await formViewLog.GetResultAsync();
            if (resultLog is IActionResult actionResultLog)
                return actionResultLog;

            ViewBag.ContentHeader = result.HtmlContent;
            ViewBag.ContentItem = resultItem.HtmlContent;
            ViewBag.ContentLog = resultLog.HtmlContent;

            if (submitAction == "send")
            {
                var alerts = await validate.ValidateAsync(formViewItem, codPedido);

                if (alerts.Any())
                {
                    return View("Edit", new PedidoViewModel
                    {
                        CodPedido = codPedido,
                        ValidationAlerts = alerts
                    });
                }

                await service.SendOrder(dataPanel);
                await logService.SaveLog(codPedido, "Pedido Exportado", (int)LogStatus.Send);
                TempData["Mensagem"] = "Pedido Exportado";
                return RedirectToAction("Details", new { codPedido });
            }

            var vm = await service.SavePedido(dataPanel);

            if (vm.ErrorAlert == null)
            {
                await logService.SaveLog(codPedido, "Pedido atualizado.", (int)LogStatus.Edit);
                TempData["Mensagem"] = "Pedido atualizado com sucesso!";
                return RedirectToAction("Edit", new { codPedido = vm.CodPedido });
            }

            return View("Edit", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? codPedido)
        {
            var dataPanel = await service.SetupDataPanelPedido(codPedido);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            var vm = new PedidoViewModel
            {
                DataPanel = dataPanel
            };

            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int codPedido)
        {
            var dataPanel = await service.SetupDataPanelPedido(codPedido);

            var result = await dataPanel.GetResultAsync();

            if (result is IActionResult actionResult)
                return actionResult;

            ViewBag.Content = result.HtmlContent;

            var vm = await service.SavePedido(dataPanel);

            if (vm.ErrorAlert == null)
            {
                await logService.SaveLog((int)vm.CodPedido, "Pedido Adicionado", (int)LogStatus.Insert);
                TempData["Mensagem"] = "Pedido salvo com sucesso!";
                return RedirectToAction("Edit", new { codPedido = vm.CodPedido });
            }

            return View("Create", vm);
        }

        //public async Task<IActionResult> SendOrder(int codPedido)
        //{
        //    var dataPanel = await service.SetupDataPanelPedido(codPedido);

        //    await service.SendOrder(dataPanel);
        //    await logService.SaveLog(codPedido, "Pedido Exportado", (int)LogStatus.Send);

        //    TempData["Mensagem"] = "Pedido Exportado";
        //    return RedirectToAction("Details", new {codPedido});
        //}
    }
}
