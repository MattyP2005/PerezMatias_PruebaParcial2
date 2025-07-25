using System.Net.Http.Json;
using System.Text.Json;

namespace GestionTareas.API.Consumer
{
    public class Crud<T> where T : class
    {
        private readonly HttpClient _http;
        private readonly string _baseUrl;
        private readonly string _endpoint;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public Crud(HttpClient httpClient, string baseUrl, string endpoint)
        {
            _http = httpClient;
            _baseUrl = baseUrl.TrimEnd('/');
            _endpoint = endpoint.Trim('/');
        }

        public async Task<List<T>> ObtenerTodosAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var res = await _http.GetAsync($"{_baseUrl}/api/{_endpoint}");
            res.EnsureSuccessStatusCode();
            return await res.Content.ReadFromJsonAsync<List<T>>(_jsonOptions) ?? new List<T>();
        }

        public async Task<T?> ObtenerPorIdAsync(int id, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var res = await _http.GetAsync($"{_baseUrl}/api/{_endpoint}/{id}");
            return res.IsSuccessStatusCode ? await res.Content.ReadFromJsonAsync<T>(_jsonOptions) : null;
        }

        public async Task<int?> CrearAsync(T entidad, string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var res = await _http.PostAsJsonAsync($"{_baseUrl}/api/{_endpoint}", entidad);
            if (!res.IsSuccessStatusCode) return null;

            var json = await res.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("id", out var idProp))
                return idProp.GetInt32();

            return null;
        }


    }
}
