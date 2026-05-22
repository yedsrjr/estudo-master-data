namespace Domain.Models.ViewModels
{
    public class DashResumoViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Qtd { get; set; }
        public decimal TotalPedidos { get; set; }
    }
}
