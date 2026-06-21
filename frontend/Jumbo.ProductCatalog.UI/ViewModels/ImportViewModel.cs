using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Jumbo.ProductCatalog.UI.Models;

namespace Jumbo.ProductCatalog.UI.ViewModels;

public sealed class ImportViewModel(IHttpClientFactory httpClientFactory)
{
    public string? Json { get; set; }
    public ImportResult? Result { get; private set; }
    public bool IsSubmitting { get; private set; }
    public string? Error { get; private set; }

    public async Task ImportAsync()
    {
        Result = null;
        Error = null;
        try { JsonDocument.Parse(Json!); }
        catch (JsonException ex)
        {
            Error = $"Invalid JSON: {ex.Message}";
            return;
        }

        IsSubmitting = true;
        var client = httpClientFactory.CreateClient("ProductApi");
        var content = new StringContent(Json!, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/products/import", content);
        IsSubmitting = false;
        if (response.IsSuccessStatusCode)
        {
            Result = await response.Content.ReadFromJsonAsync<ImportResult>();
        }
        else
        {
            Error = await response.ReadApiErrorAsync();
        }
    }
}
