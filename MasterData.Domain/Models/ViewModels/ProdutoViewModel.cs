using JJConsulting.Html.Bootstrap.Components;
using JJMasterData.Core.UI.Components;

namespace Domain.Models.ViewModels
{
    public class ProdutoViewModel
    {
        public JJDataPanel? DataPanel { get; set; }
        public JJAlert? ErrorAlert { get; set; } = new JJAlert();
        public JJAlert? SuccessAlert { get; set; } = new JJAlert();
    }
}
