using Microsoft.IdentityModel.Clients.ActiveDirectory;
using PersistenceContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PersistenceContext.Bases
{
    public abstract class HttpClientFactoryBase : Interfaces.IHttpClientFactory
    {
        private readonly string _url;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _authority;
        private readonly string _crmId;
        private readonly string _userId;
        private HttpClient _client;

        private readonly object lockObj = new object();
        private readonly int refreshInterval = 15 * 60;

        private static readonly Dictionary<string, DateTime> lastActiveClientsRefreshes = new Dictionary<string, DateTime>();
        private static readonly Dictionary<string, HttpClient> activeClients = new Dictionary<string, HttpClient>();

        public HttpClientFactoryBase(string url, string clientId, string clientSecret, string authority, string crmId, string userId = "")
        {
            _url = url;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _authority = authority;
            _crmId = crmId;
            _userId = userId;
        }

        public HttpClient CrmClient
        {
            get
            {
                if (_client != null)
                    return _client;

                _client = GetInstance(_userId);
                return _client;
            }
        }

        public virtual HttpClient GetInstance(string userId)
        {
            lock (lockObj)
            {
                if (!activeClients.TryGetValue(userId, out HttpClient client))
                {
                    client = CreateInstance(userId);
                    activeClients.Add(userId, client);
                }

                if (!lastActiveClientsRefreshes.TryGetValue(userId, out DateTime lastRefresh))
                {
                    lastActiveClientsRefreshes.Add(userId, DateTime.Now);
                }

                if ((DateTime.Now - lastRefresh).TotalSeconds >= refreshInterval)
                {
                    client = CreateInstance(userId);
                    activeClients.Remove(userId);
                    activeClients.Add(userId, client);

                    lastActiveClientsRefreshes.Remove(userId);
                    lastActiveClientsRefreshes.Add(userId, DateTime.Now);
                }

                return client;
            }
        }

        private HttpClient CreateInstance()
        {
            ClientCredential credential = new ClientCredential(_clientId, _clientSecret);
            string authUri = _authority + _crmId + "/oauth2/authorize";

            AuthenticationContext authContext = new AuthenticationContext(authUri, false);
            AuthenticationResult result = authContext.AcquireToken(_url, credential);

            HttpClient httpClient = new HttpClient(new HttpClientRetryHandler(new HttpClientHandler()))
            {
                BaseAddress = new Uri(_url),
                Timeout = new TimeSpan(0, 2, 0)
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Add("Prefer", "odata.include-annotations=\"*\"");

            return httpClient;
        }

        private HttpClient CreateInstance(string userId)
        {
            HttpClient httpClient = CreateInstance();
            if (!string.IsNullOrEmpty(userId))
            {
                const string CALLERID = "MSCRMCallerID";
                httpClient.DefaultRequestHeaders.Add(CALLERID, userId);
            }
            return httpClient;
        }
    }
}
