using System.Text;
using System.Text.Json;

namespace KfkAdmin.Extensions.Kafka;

public static class HeaderExtension
{
    public static string HeaderToJson(this Dictionary<string, byte[]> headers)
    {
        var base64Headers = headers.ToDictionary(
            kvp => kvp.Key,
            kvp => Encoding.UTF8.GetString(kvp.Value)
        );

        // Сериализация в JSON
        return JsonSerializer.Serialize(base64Headers);
    }
}