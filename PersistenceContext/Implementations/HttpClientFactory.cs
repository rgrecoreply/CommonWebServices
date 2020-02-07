using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using PersistenceContext.Bases;
using PersistenceContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceContext.Implementations
{
    public class HttpClientFactory : HttpClientFactoryBase, IPersistenceContext
    {
        public HttpClientFactory(string url, string clientId, string clientSecret, string authority, string crmId) : base(url, clientId, clientSecret, authority, crmId) { }

        public string CallAction<T>(string actionName, Guid entityId, object inputParams) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public string CallGenericAction<T>(string actionName, object inputParams) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public string CallWorkflow<T>(Guid workflowId, object body) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public Guid Create<T>(T entity) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public bool Delete<T>(Guid id) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public bool Exist<T>(Guid id) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public T Get<T>(QueryHelper<T> query) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public T Get<T>(Guid id) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>() where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetMultiple<T>(QueryHelper<T> query) where T : CrmEntity
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(T entity) where T : CrmEntity
        {
            throw new NotImplementedException();
        }
    }
}
