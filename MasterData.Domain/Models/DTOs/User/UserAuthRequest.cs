using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Domain.Models.DTOs.User
{
    public class UserAuthRequest
    {
        [JsonProperty("Name")]
        [Required(ErrorMessage = "Name é obrigatório")]
        public string Name { get; set; }


        [JsonProperty("Email")]
        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }


        [JsonProperty("PasswordHash")]
        [Required(ErrorMessage = "Password é obrigatório")]
        public string PasswordHash { get; set; }


        [JsonProperty("Role")]
        [Required(ErrorMessage = "Role é obrigatório")]
        public string Role { get; set; }
    }
}
