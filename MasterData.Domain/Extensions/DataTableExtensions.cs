using Newtonsoft.Json;
using System.Data;

namespace MasterData.Domain.Extensions;

public static class DataTableExtensions
{
    public static List<T>? ToModelList<T>(this DataTable dataTable, JsonSerializerSettings? jsonSerializerSettings = null)
    {
        var serialized = JsonConvert.SerializeObject(dataTable, jsonSerializerSettings);

        return JsonConvert.DeserializeObject<List<T>>(serialized, jsonSerializerSettings);
    }
}