using CRMWebCommon.Helpers;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebCommon.EntityWrappers
{
    public class CrmEntity
    {
        private readonly Guid _id;
        private readonly string _logicalName;
        private Entity entity;
        public CrmEntity(string logicalName) { _logicalName = logicalName; }

        public CrmEntity(string logicalName, Guid id) { _logicalName = logicalName; _id = id; }

        public CrmEntity(Entity ent) { _logicalName = ent.LogicalName; _id = ent.Id; entity = ent; }

        [JsonIgnore]
        public Guid Id { get { return entity.Id; } }

        [JsonIgnore]
        [DataMember(Name = "")]
        public string LogicalName { get { return _logicalName; } }
        [JsonIgnore]
        [DataMember(Name = "")]
        public string LogicalNameId { get { return _logicalName + "id"; } }

        [DataMember(Name = "createdby")]
        public EntityReference CreatedBy { get { return GetValue<EntityReference>("createdby"); } set { SetValue("createdby", value); } }

        [DataMember(Name = "modifiedon")]
        public DateTime ModifiedOn { get { return GetValue<DateTime>("createdon"); } set { SetValue("modifiedon", value); } }

        [DataMember(Name = "modifiedby")]
        public EntityReference ModifiedBy { get { return GetValue<EntityReference>("modifiedby"); } set { SetValue("modifiedby", value); } }

        [DataMember(Name = "createdon")]
        public DateTime CreatedOn { get { return GetValue<DateTime>("createdon"); } set { SetValue("createdon", value); } }

        [DataMember(Name = "statecode")]
        public OptionSetValue StateCode { get { return GetValue<OptionSetValue>("statecode"); } set { SetValue("statecode", value); } }

        [DataMember(Name = "statuscode")]
        public OptionSetValue StatusCode { get { return GetValue<OptionSetValue>("statuscode"); } set { SetValue("statuscode", value); } }

        [DataMember(Name = "ownerid")]
        public EntityReference Proprietario { get { return GetValue<EntityReference>("ownerid"); } set { SetValue("ownerid", value); } }

        public virtual T GetValue<T>(string field, bool aliased = false)
        {
            if (entity.Contains(field))
                return aliased ? (T)entity.GetAttributeValue<AliasedValue>(field).Value : entity.GetAttributeValue<T>(field);

            return default(T);
        }

        public virtual bool SetValue(string field, object value)
        {
            if (value == null)
                return false;

            if (value.GetType().Equals(typeof(DateTime)) && ((DateTime)value).Equals(default(DateTime)))
                return false;

            entity[field] = value;
            return true;
        }

        public Entity ToEntity()
        {
            return entity;
        }

        public EntityReference ToEntityReference()
        {
            return entity.ToEntityReference();
        }

        public bool Equals(CrmEntity x, CrmEntity y)
        {
            return x?.Id.Equals(y.Id) ?? false;
        }

        public int GetHashCode(CrmEntity obj)
        {
            string id = obj.Id.ToString();

            id = id.Replace("-", "");
            return Int32.Parse(id);
        }

        public string GetFieldNameFromValue(object value)
        {
            if (value == null)
                return null;

            foreach (string field in this.GetAllColumns())
            {
                object inValue = this.GetValue<object>(field);

                if (inValue != null && inValue.GetType().Equals(value.GetType()) && inValue.Equals(value))
                    return field;
            }

            return null;

        }

        public virtual IEnumerable<string> GetAllColumns()
        {
            Type type = GetType();
            PropertyInfo[] pinfos = type.GetProperties();
            IList<string> cols = new List<string>();

            foreach (PropertyInfo pi in pinfos)
            {
                IEnumerable<DataMemberAttribute> attrs = pi.GetCustomAttributes<DataMemberAttribute>();

                foreach (DataMemberAttribute attr in attrs)
                {
                    cols.Add(attr.Name);
                }
            }

            return cols;
        }

        public IEnumerable<object> GetAllFieldsValue()
        {
            IList<object> objs = new List<object>();

            IEnumerable<string> columns = this.GetAllColumns();
            foreach (string col in columns)
            {
                object obj = GetValue<object>(col);

                if (obj == null)
                    continue;

                objs.Add(obj);
            }

            return objs;
        }
    }
}
