using Crawler.API.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Crawler.API.Services
{
    public class HttpClientService : IHttpClientService
    {
        public HttpClient Create()
        {
            var client = new HttpClient();
            string basicAuthHeader = Convert.ToBase64String(Encoding.ASCII.GetBytes("apikey:af8763a782210698014b3c0ed9d6b4571412f7bd944ba6758bda2277bf7ad5ca"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeader);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            using (var client = Create())
            {
                return await client.GetAsync(url);
            }
        }

        public async Task<T> Get<T>(string url)
        {
            using (var client = Create())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                return default(T);
            }
        }

        public async Task<string> GetString(string url)
        {
            using (var client = Create())
            {
                return await client.GetStringAsync(url);
            }
        }

        public async Task<TOut> Post<TIn, TOut>(string url, TIn data)
        {
            using (var client = Create())
            {
                var response = await client.PostAsync<TIn>(url, data, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TOut>(content);
                }
                return default(TOut);
            }
        }
    }
}