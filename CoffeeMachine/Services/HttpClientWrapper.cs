using System;
using System.Text.Json;
using CoffeeMachine.IServices;

namespace CoffeeMachine.Services
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<T?> GetFromJsonAsync<T>(string requestUri, JsonSerializerOptions? options = null)
        {
            return await _httpClient.GetFromJsonAsync<T>(requestUri, options);
        }
    }
}
