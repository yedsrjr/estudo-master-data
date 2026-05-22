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
    public class ClienteService(IComponentFactory factory, IEntityRepository repository, LinkGenerator linkGen,
        IHttpContextAccessor contextAcessor, DashboardService cache)
    {
        public async Task<ComponentResult> SetupFormView()
        {
            var formView = await factory.FormView.CreateAsync("Clientes");

            // Toolbar Action Add Client
            formView.GridView.AddToolbarAction(new UrlRedirectAction
            {
                Name = "addClient",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetAdd", "Clientes"),
                Tooltip = "Adicionar Cliente",
                Icon = FontAwesomeIcon.PlusCircle,
                ShowAsButton = true,
                Text = "Adicionar",
                Order = 1
            });

            // GridAction View ClientById
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "viewClient",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetClientDetails", "Clientes") + "?codClient={Id}",
                Tooltip = "Visualizar",
                Icon = FontAwesomeIcon.Eye,
                Order = 1,
                EncryptParameters = false,
                IsModal = true,
                ModalTitle = "Detalhes do Cliente",
                ModalSize = ModalSize.Small,
            });

            // GridAction Edit Client
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "updateClient",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetAdd", "Clientes") + "?codClient={Id}",
                Tooltip = "Editar",
                Icon = FontAwesomeIcon.Pencil,
                Order = 1,
                EncryptParameters = false,
                EnableExpression = "exp:{Status} = 'Ativo'"
            });

            // GridAction Update Status
            formView.GridView.AddGridAction(new SqlCommandAction
            {
                Name = "updateStatus",
                SqlCommand = @"UPDATE [dbo].[Clientes] 
                               SET [Status] = CASE [Status]
                                                  WHEN '0' THEN '1'
                                                  WHEN '1' THEN '0'
                                                END
                                WHERE [Id] = {Id}",
                Tooltip = "Ativar/Inativar",
                Icon = FontAwesomeIcon.ToggleOff,
                Order = 1,
                ConfirmationMessage = "Deseja alterar o Status desse registro?"
            });

            var result = await formView.GetResultAsync();

            return result;
        }
        public async Task<JJDataPanel> SetupDataPanelView(int codClient)
        {

            var dataPanel = await factory.DataPanel.CreateAsync("Clientes");
            
            dataPanel.PageState = PageState.View;

            await dataPanel.LoadValuesFromPkAsync(codClient);

            return dataPanel;
        }
        public async Task<JJDataPanel> SetupDataPanelAdd(int? codClient)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("Clientes");

            if (codClient.HasValue)
            {
                dataPanel.PageState = PageState.Update;
                await dataPanel.LoadValuesFromPkAsync(codClient);
            }
            else
            {
                dataPanel.PageState = PageState.Insert;
            }

            return dataPanel;
        }
        public async Task<ClienteViewModel> SaveClient(JJDataPanel dataPanel)
        {
            var vm = new ClienteViewModel
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
                cache.InvalidateDashboardCache();

                vm.SuccessAlert = new JJAlert
                {
                    Color = BootstrapColor.Success,
                    Icon = FontAwesomeIcon.CheckCircle
                };

                vm.SuccessAlert.Messages.Add("Cliente criado com sucesso");
            }

            return vm;
        }
    }
}