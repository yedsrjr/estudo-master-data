using Newtonsoft.Json;

namespace MasterData.Domain.Models.DTOs.Product
{
    public class ProductResponse
    {
        [JsonProperty("CodSku")]
        public int Id { get; set; }

        [JsonProperty("Descricao")]
        public string? Description { get; set; }

        [JsonProperty("PesoBruto")]
        public decimal? GrossWeight { get; set; }

        [JsonProperty("PesoLiquido")]
        public decimal? NetWeight { get; set; }

        [JsonProperty("Quantidade")]
        public int? Quantity { get; set; }

        [JsonProperty("UpdatedAt")]
        public string? UpdatedAt { get; set; }

        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}