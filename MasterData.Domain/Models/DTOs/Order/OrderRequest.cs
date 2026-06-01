using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasterData.Domain.Models.DTOs.Order
{
    public class OrderRequest
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("CodClient")]
        [Required(ErrorMessage = "O código de cliente é obrigatório")]
        public int? IdCustomer { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("Total")]
        public decimal? Total { get; set; }

        [JsonProperty("Anexo")]
        [Required(ErrorMessage = "O anexo base64 é obrigatório")]
        public string? Document { get; set; }

        [JsonProperty("ObservacaoNF")]
        [Required(ErrorMessage = "A observação é obrigatória")]
        [StringLength(80, MinimumLength = 6, ErrorMessage = "Este campo deve conter entre 6 e 80 caracteres")]
        public string? Observation { get; set; }

        [JsonProperty("DataCriacao")]
        public DateTime InsertionDate { get; set; }
    }
}
