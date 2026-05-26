using Newtonsoft.Json;

namespace MasterData.Domain.Extensions;

public static class DictionaryExtensions
{
    public static T? ToModel<T>(this Dictionary<string, object?> dictionary, JsonSerializerSettings? jsonSerializerSettings = null)
    {
        var serialized = JsonConvert.SerializeObject(dictionary, jsonSerializerSettings);
        return JsonConvert.DeserializeObject<T>(serialized, jsonSerializerSettings);
    }

    public static T? ToModel<T>(this Dictionary<string, object?> dictionary, Newtonsoft.Json.JsonConverter converter)
    {
        return dictionary.ToModel<T>(new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                converter
            }
        });
    }
}