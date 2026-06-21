using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using Jumbo.ProductCatalog.UI.Models;

namespace Jumbo.ProductCatalog.UI.ViewModels;

public sealed class ProductFormViewModel(IHttpClientFactory httpClientFactory)
{
    public FormModel Form { get; } = new();
    public bool IsLoading { get; private set; }
    public bool IsSubmitting { get; private set; }
    public string? Error { get; private set; }

    public void ResetForm()
    {
        Form.Code = "";
        Form.Name = "";
        Form.Category = default;
        Form.Content = null;
        Form.IsActive = true;
        Error = null;
    }

    public async Task LoadProductAsync(Guid id)
    {
        IsLoading = true;
        Error = null;
        var client = httpClientFactory.CreateClient("ProductApi");
        var response = await client.GetAsync($"api/products/{id}");
        if (response.IsSuccessStatusCode)
        {
            var product = await response.Content.ReadFromJsonAsync<ProductDto>();
            if (product is not null)
            {
                Form.Code = product.Code;
                Form.Name = product.Name;
                Form.Category = product.Category;
                Form.Content = product.Content;
                Form.IsActive = product.IsActive;
            }
        }
        else
        {
            Error = await response.ReadApiErrorAsync();
        }
        IsLoading = false;
    }

    public async Task<bool> SubmitCreateAsync()
    {
        IsSubmitting = true;
        Error = null;
        var client = httpClientFactory.CreateClient("ProductApi");
        var response = await client.PostAsJsonAsync("api/products", new
        {
            Form.Code,
            Form.Name,
            Form.Category,
            Form.Content,
            Form.IsActive,
        });
        IsSubmitting = false;
        if (response.IsSuccessStatusCode) { return true; }
        Error = await response.ReadApiErrorAsync();
        return false;
    }

    public async Task<bool> SubmitUpdateAsync(Guid id)
    {
        IsSubmitting = true;
        Error = null;
        var client = httpClientFactory.CreateClient("ProductApi");
        var response = await client.PutAsJsonAsync($"api/products/{id}", new
        {
            Form.Name,
            Form.Category,
            Form.Content,
            Form.IsActive,
        });
        IsSubmitting = false;
        if (response.IsSuccessStatusCode) { return true; }
        Error = await response.ReadApiErrorAsync();
        return false;
    }

    public sealed class FormModel
    {
        [Required][MaxLength(50)] public string Code { get; set; } = "";
        [Required][MaxLength(50)] public string Name { get; set; } = "";
        public int Category { get; set; }
        public string? Content { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
