using System;
using System.Collections.Generic;
using System.Text;

namespace MasterData.Domain.Models.DTOs.Order
{
    public class ItemRequest
    {
        public List<ItemResponse> Items { get; set; } = new List<ItemResponse>();
    }

}
