using System.Text;

namespace AncientsAwakened.AncientsAwakenedCode.Metrics;

internal static class MetricsClient
{
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(15)
    };

    public static async Task<bool> Upload(string json)
    {
        try
        {
            using HttpRequestMessage request = new(
                HttpMethod.Post,
                MetricsEndpoint.ProjectUrl);

            request.Headers.Add(
                "apikey",
                MetricsEndpoint.PublishableKey);

            request.Headers.Add(
                "Prefer",
                "return=minimal");

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response =
                await HttpClient.SendAsync(request);

            string responseBody =
                await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(
                    $"[AncientsAwakened] Metrics upload succeeded: " +
                    $"{(int)response.StatusCode} {response.StatusCode}");

                return true;
            }

            Console.WriteLine(
                $"[AncientsAwakened] Metrics upload FAILED: " +
                $"{(int)response.StatusCode} {response.StatusCode}\n" +
                responseBody);

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[AncientsAwakened] Metrics upload exception:\n{ex}");

            return false;
        }
    }
}