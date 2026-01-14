using System.Net.Http.Json;
using DigitalSignage.Shared.DTOs;

namespace DigitalSignage.Client.Services;

public class TemplateService
{
    private readonly HttpClient _httpClient;

    public TemplateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TemplateDto>> GetAllTemplatesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TemplateDto>>("api/templates");
        return result ?? new List<TemplateDto>();
    }

    public async Task<TemplateDto?> GetTemplateByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TemplateDto>($"api/templates/{id}");
    }

    public async Task<List<TemplateDto>> GetActiveTemplatesAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TemplateDto>>("api/templates/active");
        return result ?? new List<TemplateDto>();
    }

    public async Task<TemplateDto?> CreateTemplateAsync(CreateTemplateDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/templates", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TemplateDto>();
        }
        return null;
    }

    public async Task<TemplateDto?> UpdateTemplateAsync(int id, UpdateTemplateDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/templates/{id}", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TemplateDto>();
        }
        return null;
    }

    public async Task<bool> DeleteTemplateAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/templates/{id}");
        return response.IsSuccessStatusCode;
    }
}
