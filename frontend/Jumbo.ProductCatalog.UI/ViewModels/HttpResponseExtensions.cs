using System.Net.Http.Json;
using Jumbo.ProductCatalog.UI.Models;

namespace Jumbo.ProductCatalog.UI.ViewModels;

internal static class HttpResponseExtensions
{
    internal static async Task<string> ReadApiErrorAsync(this HttpResponseMessage response)
    {
        ProblemDetailsDto? pd = null;
        try { pd = await response.Content.ReadFromJsonAsync<ProblemDetailsDto>(); } catch { }
        var message = pd?.Detail ?? pd?.Title ?? $"Error {(int)response.StatusCode}";
        if (response.Headers.TryGetValues("X-Correlation-ID", out var ids))
        {
            message += $" (ref: {ids.FirstOrDefault()})";
        }
        return message;
    }
}
