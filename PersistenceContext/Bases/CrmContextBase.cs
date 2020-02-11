using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using PersistenceContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceContext.Bases
{
    public abstract class CrmContextBase : ICrmContext
    {
        private readonly object lockObj = new object();
        private readonly int refreshInterval = 15 * 60;
        private readonly string _templateUser = "__usr__";
        private readonly string _templatePssw = "__pssw__";

        private static readonly Dictionary<string, DateTime> lastActiveContextRefreshes = new Dictionary<string, DateTime>();
        private static readonly Dictionary<string, IOrganizationService> activeContext = new Dictionary<string, IOrganizationService>();
        private IOrganizationService _crmService;

        public IOrganizationService CrmService
        {
            get
            {
                if (_crmService != null)
                    return _crmService;

                _crmService = GetService();
                return _crmService;
            }
        }

        public IOrganizationService GetService(string userId)
        {
            lock (lockObj)
            {
                if (!activeContext.TryGetValue(userId, out IOrganizationService context)) { }

                if (!lastActiveContextRefreshes.TryGetValue(userId, out DateTime lastRefresh))
                {
                    lastActiveContextRefreshes.Add(userId, DateTime.Now);
                }

                if ((DateTime.Now - lastRefresh).TotalSeconds >= refreshInterval)
                {
                    lastActiveContextRefreshes.Remove(userId);
                    activeContext.Remove(userId);
                    return null;
                }

                return context;
            }
        }

        public IOrganizationService GetService()
        {
            string connectionStringName = ConfigurationManager.AppSettings["CrmConnectionStringName"];
            return GetServiceByConnectionStringName(connectionStringName);
        }

        public IOrganizationService GetService(string username, string password)
        {
            string connectionStringName = ConfigurationManager.AppSettings["CrmAuthentication"];
            connectionStringName = connectionStringName.Replace(_templateUser, username).Replace(_templatePssw, password);
            return GetServiceByConnectionStringName(connectionStringName);
        }

        public IOrganizationService GetServiceByConnectionStringName(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return GetCrmServiceByConnectionString(connectionString);
        }

        public IOrganizationService GetCrmServiceByConnectionString(string connectionString)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient serviceClient = new CrmServiceClient(connectionString);

            IOrganizationService service = serviceClient.OrganizationWebProxyClient != null ? serviceClient.OrganizationWebProxyClient : (IOrganizationService)serviceClient.OrganizationServiceProxy;
            
            if(service != null)
            {
                activeContext.Add(serviceClient.CallerId.ToString(), service);
                lastActiveContextRefreshes.Add(serviceClient.CallerId.ToString(), DateTime.Now);
            }

            return service;
        }
    }
}
