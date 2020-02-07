
using System.Net.Http;

namespace PersistenceContext.Interfaces
{
    public interface IHttpClientFactory
    {
        HttpClient GetInstance(string userId);
    }
}
