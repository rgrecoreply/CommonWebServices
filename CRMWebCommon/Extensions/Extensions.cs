using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CRMWebCommon.Extensions
{
    public static class Extensions
    {
        public static IEnumerable<Guid> CreateMassive<T>(this IOrganizationService service, IEnumerable<T> objs) where T : CrmEntity
        {
            List<Guid> guids = new List<Guid>();
            foreach (T obj in objs)
                guids.Add(service.Create(obj as Entity));

            return guids;
        }

        public static void UpdateMassive<T>(this IOrganizationService service, IEnumerable<T> objs) where T : CrmEntity
        {
            foreach (T obj in objs)
                service.Update(obj as Entity);
        }

        public static void DeleteMassive<T>(this IOrganizationService service, IEnumerable<T> objs) where T : CrmEntity
        {
            foreach (T obj in objs)
                service.Delete(obj.LogicalName, obj.Id);
        }

        public static T Retrieve<T>(this IOrganizationService service, Guid id, params Expression<Func<T, Object>>[] lambda) where T : CrmEntity
        {
            return QueryHelper<T>.CreateInstance().GetEntity(service, id, lambda);
        }
        
        public static T Retrieve<T>(this IOrganizationService service, Guid id) where T : CrmEntity
        {
            return QueryHelper<T>.CreateInstance().AddCondition(x => x.LogicalNameId, ConditionOperator.Equal, id).AllColumns().GetEntities(service).FirstOrDefault();
        }
        public static IEnumerable<T> RetrieveMultiple<T>(this IOrganizationService service, QueryHelper<T> query) where T : CrmEntity
        {
            return query.GetEntities(service);
        }

        public static Guid Create<T>(this IOrganizationService service, T entity) where T : CrmEntity
        {
            return service.Create(entity.ToEntity());
        }

        public static void Delete<T>(this IOrganizationService service, T entity) where T : CrmEntity
        {
            service.Delete(entity.LogicalName, entity.Id);
        }
        
        public static void Update<T>(this IOrganizationService service, T entity) where T : CrmEntity
        {
            service.Update(entity.ToEntity());
        }

        public static string GetAttributeName<T>(this T entity, Expression<Func<T, object>> lambda) where T : CrmEntity
        {
            return AttributeHelper.GetAttributeName<T, object>(lambda);
        }

        public static DateTime ConvertTZ(this DateTime fromCrm)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(fromCrm, tz);
        }

        public static IEnumerable<object> ForEach<T>(this IEnumerable<object> objList, Action<object> func)
        {
            foreach (object element in objList)
                func(element);

            return objList;
        }

    }
}
