using System.Net.Http;

namespace Persistence.Interfaces
{
    public interface IHttpClientFactory
    {
        HttpClient GetInstance();
        HttpClient GetInstance(string userId);
    }
}
