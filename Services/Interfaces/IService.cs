using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IService<T> where T : CrmEntity
    {
        T Get(QueryHelper<T> query);
        T Get(Guid id);
        IEnumerable<T> GetMultiple(QueryHelper<T> query);
        IEnumerable<T> GetAll();
        Guid Create(T entity);
        IEnumerable<Guid> Create(IEnumerable<T> entities);
        bool Update(T entity);
        bool Update(IEnumerable<T> entities);
        bool Delete(Guid id);
        bool Delete(IEnumerable<Guid> ids);
        bool Delete(QueryHelper<T> query);
        bool Exist(Guid id);

        string CallAction(string actionName, Guid entityId, object inputParams);
        string CallGenericAction(string actionName, object inputParams);
        string CallWorkflow(Guid workflowId, object body);
    }
}
