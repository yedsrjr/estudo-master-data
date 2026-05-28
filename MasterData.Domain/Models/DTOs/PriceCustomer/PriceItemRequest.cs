using Fluid.Parser;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Domain.Models.DTOs.PriceCustomer
{
    public class PriceItemRequest
    {
        [JsonProperty("CodClient")]
        public int IdClient { get; set; }

        [JsonProperty("CodProduto")]
        [Required(ErrorMessage = "O código do item é obrigatório")]
        public int IdProduct { get; set; }

        [JsonProperty("ValorUnit")]
        [Required(ErrorMessage = "O valor unitário é obrigatório")]
        public decimal UnitValue { get; set; }

        [JsonProperty("DataInclusao")]
        public DateTime InsertionDate { get; set; }
    }
}
