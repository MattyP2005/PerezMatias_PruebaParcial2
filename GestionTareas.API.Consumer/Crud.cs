using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestionTareas.API.Consumer
{
    public class Crud<T> where T : class
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _endpoint;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public Crud(HttpClient httpClient, string baseUrl, string endpoint)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            _endpoint = endpoint.Trim('/');
        }

        // Método genérico para POST con DTOs o cualquier tipo
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string relativeUrl, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/{relativeUrl}", data);
            if (!response.IsSuccessStatusCode)
                return default;
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        private void SetAuthorization(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<T>> GetAllAsync(string endpoint, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>(_jsonOptions) ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync(string endpoint, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        public async Task<bool> PostAsync(string endpoint, T entidad, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.PostAsJsonAsync(endpoint, entidad);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string endpoint, T entidad, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.PutAsJsonAsync(endpoint, entidad);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(string endpoint, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }
    }
}