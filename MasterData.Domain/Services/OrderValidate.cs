using JJConsulting.FontAwesome;
using JJConsulting.Html.Bootstrap.Components;
using JJConsulting.Html.Bootstrap.Models;
using JJMasterData.Core.UI.Components;

namespace MasterData.Domain.Services;

public class OrderValidate
{
    public async Task<List<JJAlert>> ValidateAsync(JJFormView formViewItem, int codPedido)
    {
        var alerts = new List<JJAlert>();

        formViewItem.GridView.SetCurrentFilter("PedidoId", codPedido);
        var gridItemValues = await formViewItem.GridView.GetGridValuesAsync();

        if (gridItemValues == null || gridItemValues.Count == 0)
        {
            alerts.Add(CreateAlert(new[] { "O pedido deve possuir ao menos 1 item." }));
            return alerts;
        }

        var gridItemErrors = formViewItem.GridView.ValidateGridFields(gridItemValues);
        if (gridItemErrors.Count > 0)
            alerts.Add(CreateAlert(gridItemErrors.Values));

        return alerts;
    }

    private static JJAlert CreateAlert(IEnumerable<string> messages)
    {
        var alert = new JJAlert
        {
            Color = BootstrapColor.Danger,
            ShowIcon = true,
            Icon = FontAwesomeIcon.ExclamationTriangle,
            Title = "Validação: Itens do Pedido"
        };
        alert.Messages.AddRange(messages);
        return alert;
    }
}