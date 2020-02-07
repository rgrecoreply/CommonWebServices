using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using CRMWebCommon.Extensions;
using PersistenceContext.Bases;
using PersistenceContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Query;

namespace PersistenceContext.Implementations
{
    public class CrmContext : CrmContextBase, IPersistenceContext
    {
        public string CallAction<T>(string actionName, Guid entityId, object inputParams) where T : CrmEntity
        {
            return null; // TO IMPLEMENT
        }

        public string CallGenericAction<T>(string actionName, object inputParams) where T : CrmEntity
        {
            return null; // TO IMPLEMENT
        }

        public string CallWorkflow<T>(Guid workflowId, object body) where T : CrmEntity
        {
            return null; // TO IMPLEMENT
        }

        public Guid Create<T>(T entity) where T : CrmEntity
        {
            return CrmService.Create(entity);
        }

        public bool Delete<T>(Guid id) where T : CrmEntity
        {
            try
            {
                CrmService.Delete(AttributeHelper.GetAttributeName<T, string>(x => x.LogicalName), id);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }

        public bool Exist<T>(Guid id) where T : CrmEntity
        {
            return CrmService.Retrieve<T>(id, x => x.LogicalNameId) != null;
        }

        public T Get<T>(QueryHelper<T> query) where T : CrmEntity
        {
            return Activator.CreateInstance(typeof(T), CrmService.RetrieveMultiple(query).FirstOrDefault()) as T;
        }

        public T Get<T>(Guid id) where T : CrmEntity
        {
            return Activator.CreateInstance(typeof(T), CrmService.Retrieve(AttributeHelper.GetAttributeName<T, string>(x => x.LogicalName), id, new ColumnSet(true))) as T;
        }

        public IEnumerable<T> GetAll<T>() where T : CrmEntity
        {
            return CrmService.RetrieveMultiple(new QueryExpression()
            {
                EntityName = AttributeHelper.GetAttributeName<T, string>(x => x.LogicalName),
                ColumnSet = new ColumnSet(true),
                NoLock = true
            }).Entities.Select(x => Activator.CreateInstance(typeof(T), x) as T);
        }

        public IEnumerable<T> GetMultiple<T>(QueryHelper<T> query) where T : CrmEntity
        {
            return CrmService.RetrieveMultiple(query);
        }

        public bool Update<T>(T entity) where T : CrmEntity
        {
            try
            {
                CrmService.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
