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

        public IOrganizationService GetService()
        {
            string connectionStringName = ConfigurationManager.AppSettings["CrmConnectionStringName"];
            return GetService(connectionStringName);
        }

        public IOrganizationService GetService(string connectionStringName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            return GetCrmServiceByConnectionString(connectionString);
        }

        public IOrganizationService GetCrmServiceByConnectionString(string connectionString)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient serviceClient = new CrmServiceClient(connectionString);

            return serviceClient.OrganizationWebProxyClient != null ? serviceClient.OrganizationWebProxyClient : (IOrganizationService)serviceClient.OrganizationServiceProxy;
        }
    }
}
