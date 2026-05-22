namespace Domain.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int? ClientCount { get; set; }
        public int? OrderCount { get; set; }
        public int? ProductCount { get; set; }
        public List<DashResumoViewModel> OrdersByStatus { get; set; } = [];
        public decimal OrdersTotalGeral => OrdersByStatus.Sum(x => x.TotalPedidos);
    }
}
