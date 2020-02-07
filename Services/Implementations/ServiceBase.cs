using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using PersistenceContext.Interfaces;
using Services.Interfaces;

namespace Services.Implementation
{
    public abstract class ServiceBase<T> : IService<T> where T : CrmEntity
    {
        private readonly IPersistenceContext _context;
        public ServiceBase(IPersistenceContext context)
        {
            _context = context;
        }

        public virtual string CallAction(string actionName, Guid entityId, object inputParams)
        {
            return _context.CallAction<T>(actionName, entityId, inputParams);
        }

        public virtual string CallGenericAction(string actionName, object inputParams)
        {
            return _context.CallGenericAction<T>(actionName, inputParams);
        }

        public virtual string CallWorkflow(Guid workflowId, object body)
        {
            return _context.CallWorkflow<T>(workflowId, body);
        }

        public virtual Guid Create(T entity)
        {
            return _context.Create(entity);
        }

        public virtual IEnumerable<Guid> Create(IEnumerable<T> entities)
        {
            return entities.Select(x => Create(x));
        }

        public virtual bool Delete(Guid id)
        {
            return _context.Delete<T>(id);
        }

        public virtual bool Delete(IEnumerable<Guid> ids)
        {
            return ids.All(x => Delete(x));
        }

        public virtual bool Delete(QueryHelper<T> query)
        {
            return GetMultiple(query).All(x => Delete(x.Id));
        }

        public virtual bool Exist(Guid id)
        {
            return _context.Exist<T>(id);
        }

        public virtual T Get(QueryHelper<T> query)
        {
            return _context.Get(query);
        }

        public virtual T Get(Guid id)
        {
            return _context.Get<T>(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.GetAll<T>();
        }

        public virtual IEnumerable<T> GetMultiple(QueryHelper<T> query)
        {
            return _context.GetMultiple(query);
        }

        public virtual bool Update(T entity)
        {
            return _context.Update(entity);
        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            return entities.All(x => Update(x));
        }
    }
}
