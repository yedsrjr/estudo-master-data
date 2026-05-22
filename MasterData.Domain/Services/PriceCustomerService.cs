using Domain.Models.ViewModels;
using JJConsulting.FontAwesome;
using JJConsulting.Html.Bootstrap.Components;
using JJConsulting.Html.Bootstrap.Models;
using JJMasterData.Commons.Data.Entity.Repository.Abstractions;
using JJMasterData.Core.DataDictionary.Models;
using JJMasterData.Core.DataDictionary.Models.Actions;
using JJMasterData.Core.UI.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Domain.Services
{
    public class PriceCustomerService(IComponentFactory factory, IEntityRepository repository, LinkGenerator linkGen,
        IHttpContextAccessor contextAcessor)
    {
        public async Task<ComponentResult> SetupFormView()
        {
            var formView = await factory.FormView.CreateAsync("TabelaPrecos");

            // Toolbar Action Add Client
            formView.GridView.AddToolbarAction(new UrlRedirectAction
            {
                Name = "addPrice",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Create", "PriceCustomer"),
                Tooltip = "Adicionar preço para Cliente",
                Icon = FontAwesomeIcon.PlusCircle,
                ShowAsButton = true,
                Text = "Adicionar",
                Order = 1
            });

            // GridAction View ClientById
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "viewPrice",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "Details", "PriceCustomer") + "?customerId={Id}",
                Tooltip = "Visualizar",
                Icon = FontAwesomeIcon.Eye,
                Order = 1,
                EncryptParameters = false,
                IsModal = true,
                ModalTitle = "Detalhes dos valores",
                ModalSize = ModalSize.Small,
            });

            var result = await formView.GetResultAsync();

            return result;
        }

        public async Task<JJDataPanel> SetupDataPanelDetails(int customerId)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("TabelaPrecos");

            dataPanel.PageState = PageState.View;

            var result = dataPanel.LoadValuesFromPkAsync(customerId);

            return dataPanel;
        }

        public async Task<JJDataPanel> SetupDataPanelCreate()
        {
            var dataPanel = await factory.DataPanel.CreateAsync("TabelaPrecos");

            dataPanel.PageState = PageState.Insert;

            return dataPanel;
        }

        public async Task<PanelViewModel> SavePrice(JJDataPanel dataPanel)
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

                vm.SuccessAlert = new JJAlert
                {
                    Color = BootstrapColor.Success,
                    Icon = FontAwesomeIcon.CheckCircle
                };

                vm.SuccessAlert.Messages.Add("Preço ajustado com sucesso");
            }

            return vm;
        }
    }
}
