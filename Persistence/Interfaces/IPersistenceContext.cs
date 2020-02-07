
namespace Persistence.Interfaces
{
    public interface IPersistenceContext
    {
        Task<bool> Any<T>(ODataQuery<T> query) where T : CrmEntity;
        Task<T> Create<T>(T obj) where T : CrmEntity;
        Task Delete<T>(Guid id) where T : CrmEntity;
        Task<T> Find<T>(Guid id, IEnumerable<string> fields = null) where T : CrmEntity;
        Task<T> Get<T>(ODataQuery<T> query) where T : CrmEntity;
        Task<IEnumerable<T>> GetMultiple<T>(ODataQuery<T> query) where T : CrmEntity;
        Task<Guid> Insert<T>(T obj) where T : CrmEntity;
        Task Update<T>(Guid id, T obj) where T : CrmEntity;
        Task Associate<T>(string associationLogicalName, string entity1LogicalName, Guid id1, string entity2LogicalName, Guid id2) where T : CrmEntity;
        Task<string> CallAction<T>(string actionName, Guid entityId, object inputParams) where T : CrmEntity;
        Task<string> CallGenericAction<T>(string actionName, object inputParams) where T : CrmEntity;
        Task<string> CallWorkflow<T>(Guid workflowId, object body) where T : CrmEntity;
        Task<IEnumerable<T>> GetByView<T>(string viewName, ODataQuery<T> query) where T : CrmEntity;
    }
}
