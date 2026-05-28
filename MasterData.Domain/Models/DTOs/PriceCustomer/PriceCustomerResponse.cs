using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MasterData.Domain.Models.DTOs.PriceCustomer
{
    public class PriceCustomerResponse
    {
        [JsonProperty("CodClient")]
        public int IdClient { get; set; }

        [JsonProperty("CodProduto")]
        public int IdProduct { get; set; }

        [JsonProperty("ValorUnit")]
        public decimal UnitValue { get; set; }

        [JsonProperty("DataInclusao")]
        public DateTime InsertionDate { get; set; }
    }

}
