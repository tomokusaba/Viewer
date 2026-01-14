using System.Net.Http.Json;
using DigitalSignage.Shared.DTOs;

namespace DigitalSignage.Client.Services;

public class TagService
{
    private readonly HttpClient _httpClient;

    public TagService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<TagDto>> GetAllTagsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<TagDto>>("api/tags");
        return result ?? new List<TagDto>();
    }

    public async Task<TagDto?> GetTagByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<TagDto>($"api/tags/{id}");
    }

    public async Task<TagDto?> CreateTagAsync(CreateTagDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/tags", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TagDto>();
        }
        return null;
    }

    public async Task<TagDto?> UpdateTagAsync(int id, UpdateTagDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/tags/{id}", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<TagDto>();
        }
        return null;
    }

    public async Task<bool> DeleteTagAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/tags/{id}");
        return response.IsSuccessStatusCode;
    }
}
