using System.Text;
using System.Text.Json;

namespace Sales.WEB.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsonDefaultOptions=>new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive= true,
        };

        public Repository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var responseHttp=await _httpClient.GetAsync(url);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response = await UniserializeAnswer<T>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<T>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<T>(default, true, responseHttp);
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T model)
        {
            var mesageJSON=JsonSerializer.Serialize(model);
            var messageContent = new StringContent(mesageJSON, Encoding.UTF8, "application/json");
            var responseHttp=await _httpClient.PostAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
        }

        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T model)
        {
            var mesageJSON = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(mesageJSON, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var resonse=await UniserializeAnswer<TResponse>(responseHttp, _jsonDefaultOptions);
                return new HttpResponseWrapper<TResponse>(resonse, false, responseHttp);
            }
            return new HttpResponseWrapper<TResponse>(default,!responseHttp.IsSuccessStatusCode, responseHttp);
        }

        private async Task<T> UniserializeAnswer<T>(HttpResponseMessage httpResponse, JsonSerializerOptions jsonSeralizerOptions)
        {
            var respuestaString=await httpResponse.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respuestaString, jsonSeralizerOptions)!;
        }
    }
}
