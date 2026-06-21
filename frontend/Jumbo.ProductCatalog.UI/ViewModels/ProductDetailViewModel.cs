using System.Net.Http.Json;
using Jumbo.ProductCatalog.UI.Models;

namespace Jumbo.ProductCatalog.UI.ViewModels;

public sealed class ProductDetailViewModel(IHttpClientFactory httpClientFactory)
{
    public ProductDto? Product { get; private set; }
    public bool IsLoading { get; private set; }
    public string? Error { get; private set; }

    public async Task LoadAsync(Guid id)
    {
        IsLoading = true;
        Error = null;
        var client = httpClientFactory.CreateClient("ProductApi");
        var response = await client.GetAsync($"api/products/{id}");
        if (response.IsSuccessStatusCode)
        {
            Product = await response.Content.ReadFromJsonAsync<ProductDto>();
        }
        else
        {
            Error = await response.ReadApiErrorAsync();
        }
        IsLoading = false;
    }
}
