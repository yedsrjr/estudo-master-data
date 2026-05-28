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

namespace MasterData.Domain.Services
{
    public class ProdutoService(IComponentFactory factory, IEntityRepository repository, LinkGenerator linkGen,
        IHttpContextAccessor contextAcessor, DashboardService cache)
    {
        public async Task<ComponentResult> SetupFormView()
        {
            var formView = await factory.FormView.CreateAsync("Produtos");

            // ToolbarAction Add Product
            formView.GridView.AddToolbarAction(new UrlRedirectAction
            {
                Name = "addProduct",
                Text = "Adicionar",
                Tooltip = "Adicionar Produto",
                Icon = FontAwesomeIcon.PlusCircle,
                ShowAsButton = true,
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetAdd", "Produtos"),
                Order = 1
            });

            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "viewProduct",
                ModalTitle = "Detalhes do Produto",
                Tooltip = "Visualizar",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetProductDetails", "Produtos") + "?codProd={CodSku}",
                Icon = FontAwesomeIcon.Eye,
                IsModal = true,
                ModalSize = ModalSize.Small,
                EncryptParameters = false,
                Order = 1
            });

            // 
            formView.GridView.AddGridAction(new UrlRedirectAction
            {
                Name = "updateProduct",
                UrlRedirect = linkGen.GetPathByAction(contextAcessor.HttpContext!,
                "GetAdd", "Produtos") + "?codProd={CodSku}",
                Icon = FontAwesomeIcon.Pencil,
                Tooltip = "Editar",
                Order = 1,
                EncryptParameters = false,
                EnableExpression = "exp:{Status} = 'Ativo'"
            });

            // Action Update Status
            formView.GridView.AddGridAction(new SqlCommandAction
            {
                Name = "updateStatus",
                SqlCommand = @"UPDATE [dbo].[Produtos] 
                               SET [Status] = CASE [Status]
                                                  WHEN '0' THEN '1'
                                                  WHEN '1' THEN '0'
                                                END
                                WHERE [CodSku] = {CodSku}",
                Tooltip = "Ativar/Inativar",
                Icon = FontAwesomeIcon.ToggleOff,
                Order = 1,
                ConfirmationMessage = "Deseja alterar o Status desse registro?"
            });

            var result = await formView.GetResultAsync();

            return result;
        }
        public async Task<JJDataPanel> SetupDataPanelView(string codProd)
        {

            var dataPanel = await factory.DataPanel.CreateAsync("Produtos");

            dataPanel.PageState = PageState.View;

            await dataPanel.LoadValuesFromPkAsync(codProd);

            return dataPanel;
        }
        public async Task<JJDataPanel> SetupDataPanelAdd(string codProd)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("Produtos");

            if (!string.IsNullOrWhiteSpace(codProd))
            {
                dataPanel.PageState = PageState.Update;
                await dataPanel.LoadValuesFromPkAsync(codProd);
            }
            else
            {
                dataPanel.PageState = PageState.Insert;
            }

            return dataPanel;
        }
    
        public async Task<ProdutoViewModel> SaveProduct(JJDataPanel dataPanel)
        {
            var vm = new ProdutoViewModel
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

                vm.SuccessAlert.Messages.Add("Produto criado com sucesso");
            }

            return vm;
        }
    }
}
