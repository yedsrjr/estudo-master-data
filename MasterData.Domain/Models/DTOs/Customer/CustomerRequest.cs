using Fluid.Parser;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MasterData.API.Models.DTOs.Customer
{
    public class CustomerRequest
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("NomeAbreviado")]
        [Required(ErrorMessage = "O Nome Abreviado é obrigatório")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "Este campo deve conter entre 6 e 40 caracteres")]
        public string? ShortName { get; set; }
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [JsonProperty("NomeCliente")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "O CPF/CNPJ é obrigatório")]
        [JsonProperty("NumCPF")]
        public string? CpfCnpj { get; set; }
        [JsonProperty("Status")]
        public int? Status { get; set; }
        [JsonProperty("UpdatedAt")]
        public string? UpdatedAt { get; set; }
    }
}
