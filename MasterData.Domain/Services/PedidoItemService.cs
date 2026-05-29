using Domain.Models.Enums;
using Domain.Models.ViewModels;
using Domain.Repository;
using JJConsulting.FontAwesome;
using JJConsulting.Html.Bootstrap.Components;
using JJConsulting.Html.Bootstrap.Models;
using JJMasterData.Commons.Data.Entity.Repository.Abstractions;
using JJMasterData.Core.DataDictionary.Models;
using JJMasterData.Core.DataDictionary.Models.Actions;
using JJMasterData.Core.UI.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MasterData.Domain.Services
{
    public class PedidoItemService(IComponentFactory factory, IEntityRepository repository, LinkGenerator linkGen,
        IHttpContextAccessor contextAcessor, OrderRepository pedidoRepository, LogOrderService logService)
    {
        public async Task<JJFormView> SetupFormViewItem(int? idPedido)
        {
            var formView = await factory.FormView.CreateAsync("PedidoItem");

            // GridAction Include Item
            formView.GridView.AddToolbarAction(new UrlRedirectAction
            {
                Name = "addItem",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Create", "PedidoItem") + $"?codPedido={idPedido}",
                Tooltip = "Incluir",
                Icon = FontAwesomeIcon.PlusCircle,
                ShowAsButton = true,
                IsModal = false,
                ModalTitle = "Incluir item",
                ModalSize = ModalSize.ExtraLarge,
                EnableExpression = $"exp:'{{Status}}' <> {(int)OrderStatus.Send}",
                Text = "Incluir",
                Order = 1
            });

            // GridAction Remove Item
            formView.GridView.AddGridAction(new ScriptAction
            {
                Name = "removeItem",
                OnClientClick = "confirmarExclusao({Id})",
                Tooltip = "Remover Item",
                Icon = FontAwesomeIcon.TimesCircle,
                Color = BootstrapColor.Danger,
                Order = 1,
                EnableExpression = $"exp:'{{Status}}' = {(int)OrderStatus.Elaboration}"
            });

            formView.GridView.SetCurrentFilter("PedidoId", idPedido);

            return formView;
        }

        public async Task<JJDataPanel> SetupDataPanelAddItens(int idPedido)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("PedidoItem");

            dataPanel.PageState = PageState.Insert;

            dataPanel.Values["PedidoId"] = idPedido;

            var field = dataPanel.FormElement.Fields.FirstOrDefault(f => f.Name == "PedidoId");

            if (field != null)
                field.ReadOnlyExpression = "val:1";

            return dataPanel;
        }

        public async Task<JJFormView> SetupListView(int idPedido)
        {
            var form = await factory.FormView.CreateAsync("PedidoItem");

            form.GridView.SetCurrentFilter("PedidoId", idPedido);

            return form;
        }

        public async Task<PanelViewModel> SaveItem(JJDataPanel dataPanel)
        {
            var vm = new PanelViewModel
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
                var id = (int)dataPanel.Values["PedidoId"];
                vm.CodPedido = id;
                await pedidoRepository.UpdateTotalOrder(vm.CodPedido);
            }

            return vm;
        }

        public async Task DeleteItem(int id)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("PedidoItem");
            await dataPanel.LoadValuesFromPkAsync(id);
            var pedidoId = dataPanel.Values.TryGetValue("PedidoId", out var pedidoIdObj);

            var values = new Dictionary<string, object>
            {
                ["Id"] = id
            };

            await repository.DeleteAsync(dataPanel.FormElement, values);
            await logService.SaveLog(Convert.ToInt32(pedidoIdObj), "Item removido", (int)LogStatus.Delete);
        }
    }
}
