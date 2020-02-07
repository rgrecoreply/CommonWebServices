using CRMWebCommon.EntityWrappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CRMWebCommon.Helpers
{
    public class QueryHelper<T> where T: CrmEntity
    {
        
        private QueryExpression query;
        private ColumnSet columnSet;
        private List<LinkEntity> links;
        private bool active, allColumns;
        private readonly string _logicalName;

        public QueryHelper()
        {
            _logicalName = AttributeHelper.GetAttributeName<T, string>(x => x.LogicalName);
            query = new QueryExpression(_logicalName);
            columnSet = new ColumnSet();
            links = new List<LinkEntity>();
            active = true;
            allColumns = false;
        }

        public static QueryHelper<T> CreateInstance()
        {
            return new QueryHelper<T>();
        }

        private void Reset()
        {
            query = new QueryExpression(_logicalName);
            columnSet = new ColumnSet();
            links = new List<LinkEntity>();
            active = true;
        }

        public IEnumerable<T> GetEntities(IOrganizationService service)
        {
            query.NoLock = true;
            if (columnSet.Columns.Count() > 0 && !allColumns)
                query.ColumnSet = columnSet;
            else if(!allColumns)
                query.ColumnSet = new ColumnSet(false);
            if (active && !query.Criteria.FilterOperator.Equals(LogicalOperator.Or))
                query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            query.PageInfo = new PagingInfo()
            {
                Count = 5000,
                PageNumber = 1
            };

            EntityCollection entities = service.RetrieveMultiple(query);

            if (entities.MoreRecords)
                return GoPaging(service, entities);

            Reset();
            return entities.Entities.Select(x => Activator.CreateInstance(typeof(T), x) as T);
        }

        private IEnumerable<T> GoPaging(IOrganizationService service, EntityCollection entities)
        {
            DataCollection<Entity> moreEntities = entities.Entities;

            do
            {
                query.PageInfo.PageNumber++;
                query.PageInfo.PagingCookie = entities.PagingCookie;

                entities = service.RetrieveMultiple(query);

                moreEntities.AddRange(entities.Entities);

            } while (entities.MoreRecords);

            Reset();
            return moreEntities.Select(x => Activator.CreateInstance(typeof(T), x) as T);
        }

        public T GetEntity(IOrganizationService service, Guid id, params Expression<Func<T, Object>>[] fields)
        {
            List<string> columns = new List<string>();
            foreach (Expression<Func<T, Object>> lambda in fields)
                columns.Add(AttributeHelper.GetAttributeName(lambda));

            Entity entity = service.Retrieve(_logicalName, id, new ColumnSet(columns.ToArray()));

            return Activator.CreateInstance(typeof(T), entity) as T;
        }

        public QueryHelper<T> SetLogicalOperator(LogicalOperator logicalOperator)
        {
            query.Criteria.FilterOperator = logicalOperator;
            return this;
        }

        public QueryHelper<T> AddCondition(Expression<Func<T, Object>> field, ConditionOperator conditionOperator, Object value)
        {
            query.Criteria.AddCondition(AttributeHelper.GetAttributeName(field), conditionOperator, value);
            return this;
        }
        
        public QueryHelper<T> AddCondition(Expression<Func<T, Object>> field, ConditionOperator conditionOperator)
        {
            query.Criteria.AddCondition(AttributeHelper.GetAttributeName(field), conditionOperator);
            return this;
        }

        public QueryHelper<T> AddCondition(string field, ConditionOperator? conditionOperator, Object value)
        {
            if (conditionOperator == null)
                return this;

            query.Criteria.AddCondition(field, (ConditionOperator)conditionOperator, value);
            return this;
        }

        public QueryHelper<T> AddCondition(string field, ConditionOperator? conditionOperator)
        {
            if (conditionOperator == null)
                return this;

            query.Criteria.AddCondition(field, (ConditionOperator)conditionOperator);
            return this;
        }

        public QueryHelper<T> AddColumn(Expression<Func<T, Object>> field)
        {
            columnSet.AddColumn(AttributeHelper.GetAttributeName(field));
            return this;
        }

        public QueryHelper<T> AddColumns(params Expression<Func<T, Object>>[] fields)
        {
            foreach(Expression<Func<T, Object>> lambda in fields)
                AddColumn(lambda);
            return this;
        }

        public QueryHelper<T> AddColumns(IEnumerable<string> columns)
        {
            columnSet.AddColumns(columns.ToArray());
            return this;
        }

        public QueryHelper<T> AllColumns()
        {
            allColumns = true;
            query.ColumnSet = new ColumnSet(true);
            return this;
        }

        public QueryHelper<T> AddLink<TEntityTo>(Expression<Func<T, Object>> fieldFrom, Expression<Func<TEntityTo, Object>> fieldTo, string alias, string aliasTo = null) where TEntityTo : CrmEntity
        {
            LinkEntity link = new LinkEntity();
            link.LinkFromEntityName = _logicalName;
            link.LinkFromAttributeName = AttributeHelper.GetAttributeName(fieldFrom);
            link.LinkToAttributeName = AttributeHelper.GetAttributeName(fieldTo);
            link.LinkToEntityName = AttributeHelper.GetAttributeName<TEntityTo, string>(x => x.LogicalName);
            link.EntityAlias = alias;

            links.Add(link);

            if (!String.IsNullOrEmpty(aliasTo))
                links.Where(x => x.EntityAlias.Equals(aliasTo)).FirstOrDefault()?.LinkEntities.Add(link);
            else
                query.LinkEntities.Add(link);

            return this;
        }

        public QueryHelper<T> AddLink<TEntityFrom, TEntityTo>(Expression<Func<TEntityFrom, Object>> fieldFrom, Expression<Func<TEntityTo, Object>> fieldTo, string alias, string aliasTo = null) where TEntityTo : CrmEntity
        {
            Type entityFrom = typeof(TEntityFrom);
            Type crmWrapper = typeof(CrmEntity);

            if (!entityFrom.BaseType.Equals(crmWrapper))
                throw new Exception("Only Class derived from CrmEntity are supported");

            LinkEntity link = new LinkEntity();
            link.LinkFromEntityName = AttributeHelper.GetAttributeName<TEntityFrom, string>(x => (x as CrmEntity).LogicalName);
            link.LinkFromAttributeName = AttributeHelper.GetAttributeName(fieldFrom);
            link.LinkToAttributeName = AttributeHelper.GetAttributeName(fieldTo);
            link.LinkToEntityName = AttributeHelper.GetAttributeName<TEntityTo, string>(x => x.LogicalName);
            link.EntityAlias = alias;

            links.Add(link);

            if (!String.IsNullOrEmpty(aliasTo))
                links.Where(x => x.EntityAlias.Equals(aliasTo)).FirstOrDefault()?.LinkEntities.Add(link);
            else
                query.LinkEntities.Add(link);

            return this;
        }

        public QueryHelper<T> AddLinkColumns<TEntityTo>(string linkAlias, params Expression<Func<TEntityTo, Object>>[] fields) where TEntityTo : CrmEntity
        {
            LinkEntity link = links.Where(x => x.EntityAlias.Equals(linkAlias)).FirstOrDefault();

            if (link == null)
                return this;

            foreach(Expression<Func<TEntityTo, Object>> lambda in fields)
            {
                string field = AttributeHelper.GetAttributeName(lambda);
                link.Columns.AddColumn(field);
            }
            return this;
        }

        public QueryHelper<T> AddLinkCondition<TEntityTo>(string linkAlias, Expression<Func<TEntityTo, Object>> field, ConditionOperator conditionOperator, Object value) where TEntityTo : CrmEntity
        {
            LinkEntity link = links.Where(x => x.EntityAlias.Equals(linkAlias)).FirstOrDefault();

            if (link == null)
                return this;

            link.LinkCriteria.AddCondition(AttributeHelper.GetAttributeName<TEntityTo, Object>(field), conditionOperator, value);

            return this;
        }

        public QueryHelper<T> SetLinkLogicalOperator(string linkAlias, LogicalOperator logicalOperator)
        {
            LinkEntity link = links.Where(x => x.EntityAlias.Equals(linkAlias)).FirstOrDefault();

            if (link == null)
                return this;

            link.LinkCriteria.FilterOperator = logicalOperator;

            return this;
        }

        public QueryHelper<T> SetLinkJoin(string linkAlias, JoinOperator join)
        {
            LinkEntity link = links.Where(x => x.EntityAlias.Equals(join)).FirstOrDefault();

            if (link == null)
                return this;

            link.JoinOperator = join;

            return this;
        } 

        public QueryHelper<T> ForEach<TOther>(IEnumerable<TOther> objList, Action<QueryHelper<T>, TOther> func)
        {
            foreach (TOther obj in objList)
            {
                func(this, obj);
            }

            return this;
        }

        public QueryHelper<T> AddLink(LinkEntity link)
        {
            query.LinkEntities.Add(link);
            return this;
        }

        public QueryHelper<T> Deactive()
        {
            active = false;
            return this;
        }

        public QueryExpression GetQuery()
        {
            return query;
        }

    }
}
