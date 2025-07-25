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

        public Crud(HttpClient httpClient, string baseUrl, string endpoint)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            _endpoint = endpoint.Trim('/');
        }

        public async Task<List<T>> GetAllAsync(string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/{_endpoint}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<T>>() ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync(int id, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/{_endpoint}/{id}");
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<int?> PostAsync(T entity, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/api/{_endpoint}", entity);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("id", out var idProp))
                return idProp.GetInt32();

            return null;
        }

        public async Task<bool> PutAsync(int id, T entity, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.PutAsJsonAsync($"{_baseUrl}/api/{_endpoint}/{id}", entity);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(int id, string token)
        {
            SetAuthorization(token);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/{_endpoint}/{id}");
            return response.IsSuccessStatusCode;
        }

        private void SetAuthorization(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}