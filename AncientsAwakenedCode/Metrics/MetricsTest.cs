using System.Text.Json;

namespace AncientsAwakened.AncientsAwakenedCode.Metrics;

internal static class MetricsTest
{
    public static async Task SendTest()
    {
        var payload = new
        {
            schema_version = 1,
            mod_version = "TEST",
            has_foreign_content = false,
            data = new
            {
                test = true,
                message = "Hello from Ancients Awakened"
            }
        };

        string json = JsonSerializer.Serialize(payload);

        await MetricsClient.Upload(json);
    }
}