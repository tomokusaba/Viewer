using System.Net.Http.Json;
using DigitalSignage.Shared.DTOs;

namespace DigitalSignage.Client.Services;

public class ContentService
{
    private readonly HttpClient _httpClient;

    public ContentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ContentDto>> GetAllContentsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ContentDto>>("api/contents");
        return result ?? new List<ContentDto>();
    }

    public async Task<ContentDto?> GetContentByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<ContentDto>($"api/contents/{id}");
    }

    public async Task<List<ContentDto>> GetActiveContentsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<List<ContentDto>>("api/contents/active");
        return result ?? new List<ContentDto>();
    }

    public async Task<List<ContentDto>> GetContentsByTagsAsync(List<int> tagIds)
    {
        var tags = string.Join(",", tagIds);
        var result = await _httpClient.GetFromJsonAsync<List<ContentDto>>($"api/contents/by-tags?tags={tags}");
        return result ?? new List<ContentDto>();
    }

    public async Task<ContentDto?> CreateContentAsync(CreateContentDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/contents", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ContentDto>();
        }
        return null;
    }

    public async Task<ContentDto?> UpdateContentAsync(int id, UpdateContentDto dto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/contents/{id}", dto);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ContentDto>();
        }
        return null;
    }

    public async Task<bool> DeleteContentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/contents/{id}");
        return response.IsSuccessStatusCode;
    }
}
