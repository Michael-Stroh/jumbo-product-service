using System.Net.Http.Json;
using Jumbo.ProductCatalog.UI.Models;

namespace Jumbo.ProductCatalog.UI.ViewModels;

public sealed class ProductListViewModel(IHttpClientFactory httpClientFactory)
{
    public List<ProductDto>? Products { get; private set; }
    public string? Error { get; private set; }

    public async Task LoadAsync()
    {
        Products = null;
        Error = null;
        try
        {
            var client = httpClientFactory.CreateClient("ProductApi");
            Products = await client.GetFromJsonAsync<List<ProductDto>>("api/products") ?? [];
        }
        catch
        {
            Error = "Failed to load products. Please try again.";
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        Error = null;
        var client = httpClientFactory.CreateClient("ProductApi");
        var response = await client.DeleteAsync($"api/products/{id}");
        if (response.IsSuccessStatusCode)
        {
            Products = Products?.Where(p => p.Id != id).ToList();
        }
        else
        {
            Error = await response.ReadApiErrorAsync();
        }
    }
}
