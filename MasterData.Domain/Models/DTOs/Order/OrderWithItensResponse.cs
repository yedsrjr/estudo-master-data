namespace MasterData.Domain.Models.DTOs.Order
{
    public class OrderWithItensResponse
    {
        public OrderResponse Order { get; set; }
        public List<ItemResponse>? Itens { get; set; }
    }
}
