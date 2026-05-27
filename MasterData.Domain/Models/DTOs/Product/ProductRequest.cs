using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Domain.Models.DTOs.Product
{
    public class ProductRequest
    {
        [JsonProperty("CodSku")]
        public int Id { get; set; }

        [JsonProperty("Descricao")]
        [Required(ErrorMessage = "A Descrição do Item é obrigatória")]
        public string? Description { get; set; }

        [JsonProperty("PesoBruto")]
        [Required(ErrorMessage = "O Peso Bruto é obrigatório")]
        public decimal? GrossWeight { get; set; }

        [JsonProperty("PesoLiquido")]
        [Required(ErrorMessage = "O Peso Líquido é obrigatório")]
        public decimal? NetWeight { get; set; }

        [JsonProperty("Quantidade")]
        [Required(ErrorMessage = "A Quantidade é obrigatória")]
        public int? Quantity { get; set; }

        [JsonProperty("UpdatedAt")]
        public string? UpdatedAt { get; set; }

        [JsonProperty("Status")]
        public int? Status { get; set; }
    }
}
