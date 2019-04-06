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
        Task<HttpResponseMessage> Delete(string url);
        Task<TOut> Post<TIn, TOut>(string url, TIn data);
        Task<TOut> Patch<TIn, TOut>(string url, TIn data);
    }
}