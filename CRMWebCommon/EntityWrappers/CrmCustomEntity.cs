using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Serialization;

namespace CRMWebCommon.EntityWrappers
{
    public class CrmCustomEntity: CrmEntity
    {
        public CrmCustomEntity(string logicalName) : base(logicalName) { }
        public CrmCustomEntity(string logicalName, Guid id) : base(logicalName, id) { }

        public CrmCustomEntity(Entity entity) : base(entity) { }

        [DataMember(Name = "clu_name")]
        public string Nome { get { return GetValue<string>("clu_name"); } set { SetValue("clu_name", value); } }
    }
}
