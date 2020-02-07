using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersistenceContext.Interfaces
{
    public interface IPersistenceContext
    {
        T Get<T>(QueryHelper<T> query) where T : CrmEntity;
        T Get<T>(Guid id) where T : CrmEntity;
        IEnumerable<T> GetMultiple<T>(QueryHelper<T> query) where T : CrmEntity;
        IEnumerable<T> GetAll<T>() where T : CrmEntity;
        Guid Create<T>(T entity) where T : CrmEntity;
        bool Update<T>(T entity) where T : CrmEntity;
        bool Delete<T>(Guid id) where T : CrmEntity;
        bool Exist<T>(Guid id) where T : CrmEntity;
        
        string CallAction<T>(string actionName, Guid entityId, object inputParams) where T : CrmEntity;
        string CallGenericAction<T>(string actionName, object inputParams) where T : CrmEntity;
        string CallWorkflow<T>(Guid workflowId, object body) where T : CrmEntity;
    }
}
