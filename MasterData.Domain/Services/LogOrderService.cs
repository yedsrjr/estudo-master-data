using JJMasterData.Commons.Data.Entity.Repository.Abstractions;
using JJMasterData.Core.DataDictionary.Models;
using JJMasterData.Core.UI.Components;

namespace MasterData.Domain.Services
{
    public class LogOrderService(IComponentFactory factory, IEntityRepository repository)
    {
        public async Task<JJFormView> SetupFormView(int? idPedido)
        {
            var formView = await factory.FormView.CreateAsync("LogPedido");

            formView.GridView.SetCurrentFilter("IdPedido", idPedido);

            return formView;
        }

        public async Task<JJDataPanel> SetupDataPanel(int idPedido, string observation, int status)
        {
            var dataPanel = await factory.DataPanel.CreateAsync("LogPedido");

            dataPanel.PageState = PageState.Update;

            dataPanel.Values["IdPedido"] = idPedido;
            dataPanel.Values["Observacao"] = observation;
            dataPanel.Values["Status"] = status;

            return dataPanel;
        }

        public async Task SaveLog(int idPedido, string observation, int status)
        {
            var dataPanel = await SetupDataPanel(idPedido, observation, status);
            var values = await dataPanel.GetFormValuesAsync();

            await repository.SetValuesAsync(dataPanel.FormElement, values);
        }
    }
}

