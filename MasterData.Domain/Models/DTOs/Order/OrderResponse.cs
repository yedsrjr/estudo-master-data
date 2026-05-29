using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasterData.Domain.Models.DTOs.Order
{
    public class OrderResponse
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("CodClient")]
        public int? IdCustomer { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("Total")]
        public decimal? Total { get; set; }

        [JsonProperty("Anexo")]
        public string? Document { get; set; }

        [JsonProperty("ObservacaoNF")]
        public string? Observation { get; set; }

        [JsonProperty("DataCriacao")]
        public DateTime InsertionDate { get; set; }
    }
}
