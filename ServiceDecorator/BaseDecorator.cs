using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using Services.Implementation;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceDecorator
{
    public class BaseDecorator<T> : IService<T> where T : CrmEntity
    {
        private readonly IService<T> _service;

        public BaseDecorator(IService<T> service)
        {
            _service = service;
        }

        public virtual string CallAction(string actionName, Guid entityId, object inputParams)
        {
            return _service.CallAction(actionName, entityId, inputParams);
        }

        public virtual string CallGenericAction(string actionName, object inputParams)
        {
            return _service.CallGenericAction(actionName, inputParams);
        }

        public virtual string CallWorkflow(Guid workflowId, object body)
        {
            return _service.CallWorkflow(workflowId, body);
        }

        public virtual Guid Create(T entity)
        {
            return _service.Create(entity);
        }

        public virtual IEnumerable<Guid> Create(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(Guid id)
        {
            return _service.Delete(id);
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
            return _service.Exist(id);
        }

        public virtual T Get(QueryHelper<T> query)
        {
            return _service.Get(query);
        }

        public virtual T Get(Guid id)
        {
            return _service.Get(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _service.GetAll();
        }

        public virtual IEnumerable<T> GetMultiple(QueryHelper<T> query)
        {
            return _service.GetMultiple(query);
        }

        public virtual bool Update(T entity)
        {
            return _service.Update(entity);
        }

        public virtual bool Update(IEnumerable<T> entities)
        {
            return entities.All(x => Update(x));
        }
    }
}
