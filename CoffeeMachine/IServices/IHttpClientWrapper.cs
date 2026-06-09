using System;
using System.Text.Json;

namespace CoffeeMachine.IServices
{
    public interface IHttpClientWrapper
    {
        Task<T?> GetFromJsonAsync<T>(string requestUri, JsonSerializerOptions? options = null);

    }
}
