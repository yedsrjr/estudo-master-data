using System;
using System.Collections.Generic;
using System.Text;

namespace MasterData.Domain.Models.DTOs.User
{
    public record User(int Id, string Name, string Email, string Password, string[] Roles);
    
}
