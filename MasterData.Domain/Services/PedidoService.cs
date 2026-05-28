using Domain.Models.Enums;
using Domain.Models.ViewModels;
using Domain.Repository;
using JJConsulting.FontAwesome;
using JJConsulting.Html.Bootstrap.Components;
using JJConsulting.Html.Bootstrap.Models;
using JJMasterData.Commons.Data.Entity.Repository.Abstractions;
using JJMasterData.Core.DataDictionary.Models;
using JJMasterData.Core.DataDictionary.Models.Actions;
using JJMasterData.Core.DataManager.IO;
using JJMasterData.Core.UI.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MasterData.Domain.Services
{
    public class PedidoService(IComponentFactory factory,IHttpContextAccessor contextAcessor, LinkGenerator linkGen,
        IEntityRepository repository, DashboardService cache, PedidoRepository pedidoRepository,
        FormFileService fileService)
    {
        public async Task<ComponentResult> SetupFormView()
        {
            var formView = await factory.FormView.CreateAsync("Pedidos");

            // Toolbar Action Add Pedido
            formView.GridView.AddToolbarAction(new UrlRedirectAction
            {
                Name = "addPedido",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Create", "Pedidos"),
                Tooltip = "Adicionar Pedido",
                Icon = FontAwesomeIcon.PlusCircle,
                ShowAsButton = true,
                Text = "Adicionar",
                Order = 1,
            });

            // GridAction View 
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "viewPedido",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Details", "Pedidos") + "?codPedido={Id}",
                Tooltip = "Visualizar",
                EncryptParameters = false,
                Icon = FontAwesomeIcon.Eye,
                IsModal = false,
                ModalTitle = "Detalhes do Pedido",
                ModalSize = ModalSize.Small,
                Order = 1,
            });

            // GridAction Edit
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "editPedido",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Edit", "Pedidos") + "?codPedido={Id}",
                Tooltip = "Editar",
                EncryptParameters = false,
                Icon = FontAwesomeIcon.SolidFilePen,
                Color = BootstrapColor.Primary,
                EnableExpression = $"exp:'{{Status}}' = {(int)OrderStatus.Elaboration}",
                Order = 2
            });

            // GridAction Cancel Order
            formView.GridView.AddGridAction(new SqlCommandAction
            {
                Name = "cancelOrder",
                SqlCommand = "EXEC dbo.SP_CANCELAR_PEDIDO @ID = {Id};",
                Tooltip = "Cancelar Pedido",
                Icon = FontAwesomeIcon.Ban,
                Color = BootstrapColor.Danger,
                EnableExpression = $"exp:'{{Status}}' = {(int)OrderStatus.Elaboration}",
                ConfirmationMessage = "Tem certeza que deseja cancelar esse pedido?",
                Order = 3
            });
            
            var result = await formView.GetResultAsync();

            return result;
        }

        public async Task<JJDataPanel> SetupDataPanelView(int? codPedido)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("Pedidos");

            dataPanel.PageState = PageState.View;

            await dataPanel.LoadValuesFromPkAsync(codPedido);

            return dataPanel;
        }

        public async Task<JJDataPanel> SetupDataPanelPedido(int? codPedido)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("Pedidos");

            if (codPedido.HasValue)
            {
                dataPanel.PageState = PageState.Update;
                await dataPanel.LoadValuesFromPkAsync(codPedido);
                var field = dataPanel.FormElement.Fields.FirstOrDefault(f => f.Name == "CodClient");

                field.EnableExpression = "val:0";
                field.ReadOnlyExpression = "val:1";
            }
            else
            {
                dataPanel.PageState = PageState.Insert;
            }

            return dataPanel;
        }

        public async Task<JJDataPanel> SetupDataPanelView(int codPedido)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("Pedidos");

            dataPanel.PageState = PageState.View;
            
            await dataPanel.LoadValuesFromPkAsync(codPedido);

            return dataPanel;
        }

        public async Task<PedidoViewModel> SavePedido(JJDataPanel dataPanel)
        {
            var vm = new PedidoViewModel
            {
                DataPanel = dataPanel
            };

            var values = await dataPanel.GetFormValuesAsync();
            var errors = dataPanel.ValidateFields(values);

            if (errors.Count > 0)
            {
                dataPanel.Errors = errors;

                vm.ErrorAlert = new JJAlert
                {
                    Color = BootstrapColor.Danger,
                    Icon = FontAwesomeIcon.ExclamationTriangle,
                    ShowCloseButton = true,
                    ShowIcon = true
                };
                vm.ErrorAlert.Messages.AddRange(errors.Values);
            }
            else
            {
                await repository.SetValuesAsync(dataPanel.FormElement, values);

                vm.CodPedido = (int?)dataPanel.Values["Id"];

                fileService.SaveFormMemoryFiles(dataPanel.FormElement, values);

                cache.InvalidateDashboardCache();

                await pedidoRepository.UpdateTotalOrder((int)vm.CodPedido);
            }

            return vm;
        }

        public async Task SendOrder(JJDataPanel dataPanel)
        {
            dataPanel.Values["Status"] = (int)OrderStatus.Send;
            var codPedido = dataPanel.Values["Id"];

            var values = await dataPanel.GetFormValuesAsync();

            await repository.SetValuesAsync(dataPanel.FormElement, values);
            fileService.SaveFormMemoryFiles(dataPanel.FormElement, values);
            await pedidoRepository.UpdateTotalOrder((int)codPedido);
        }
    }
}
