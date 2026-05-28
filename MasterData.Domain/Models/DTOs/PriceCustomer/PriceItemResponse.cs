using Newtonsoft.Json;

namespace MasterData.Domain.Models.DTOs.PriceCustomer
{
    public class PriceItemResponse
    {
        [JsonProperty("CodProduto")]
        public int IdProduct { get; set; }

        [JsonProperty("ValorUnit")]
        public decimal UnitValue { get; set; }

        [JsonProperty("DataInclusao")]
        public DateTime InsertionDate { get; set; }
    }
}
