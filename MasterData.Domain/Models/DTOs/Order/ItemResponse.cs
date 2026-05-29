using Newtonsoft.Json;

namespace MasterData.Domain.Models.DTOs.Order
{
    public class ItemResponse
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("ItemId")]
        public int IdItem { get; set; }

        [JsonProperty("PedidoId")]
        public int IdOrder { get; set; }

        [JsonProperty("Quantidade")]
        public int Quantity { get; set; }

        [JsonProperty("PesoBruto")]
        public decimal GrossWeight { get; set; }

        [JsonProperty("PesoLiquido")]
        public decimal NetWeight { get; set; }

        [JsonProperty("ValorUnit")]
        public decimal UnitValue { get; set; }

        [JsonProperty("ValorTotal")]
        public decimal TotalValue  { get; set; }
    }
}