using Newtonsoft.Json;

namespace MasterData.Domain.Models.DTOs.Customer;

public class CustomerResponse
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("NomeAbreviado")]
    public string? ShortName { get; set; }
    [JsonProperty("NomeCliente")]
    public string? Name { get; set; }
    [JsonProperty("NumCPF")]
    public string? CpfCnpj { get; set; }
    [JsonProperty("Status")]
    public string? Status { get; set; }
    [JsonProperty("UpdatedAt")]
    public string? UpdatedAt { get; set; }
}
