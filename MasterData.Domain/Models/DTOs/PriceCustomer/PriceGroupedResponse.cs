using Newtonsoft.Json;

namespace MasterData.Domain.Models.DTOs.PriceCustomer
{
    public class PriceGroupedResponse
    {
        [JsonProperty("CodClient")]
        public int IdClient { get; set; }

        [JsonProperty("Produtos")]
        public List<PriceItemResponse> Products { get; set; } = [];
    }
}
