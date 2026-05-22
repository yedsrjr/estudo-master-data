using JJConsulting.Html.Bootstrap.Components;
using JJMasterData.Core.UI.Components;

namespace Domain.Models.ViewModels
{
    public class PanelViewModel
    {
        public JJDataPanel DataPanel { get; set; }
        public JJAlert? ErrorAlert { get; set; }
        public JJAlert? SuccessAlert { get; set; }
        public int CodPedido { get; set; }
    }
}
