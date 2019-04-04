using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.API.Interfaces
{
    public interface IHttpClientService
    {
        HttpClient Create();
        Task<HttpResponseMessage> Get(string url);
        Task<string> GetString(string url);
        Task<T> Get<T>(string url);
        Task<TOut> Post<TIn, TOut>(string url, TIn data);
    }
}