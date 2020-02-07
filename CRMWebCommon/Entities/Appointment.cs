using CRMWebCommon.EntityWrappers;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CRMWebCommon.Entities
{
	public class Appointment: CrmEntity
	{
		public Appointment() : base("appointment") { }
		public Appointment(Guid id) : base("appointment", id) { }

        public Appointment(Entity entity) : base(entity) { }

		[DataMember(Name = "appointment")]
        [JsonProperty("logicalname")]
		public new string LogicalName { get { return base.LogicalName; } }

		[DataMember(Name = "activityid")]
        [JsonIgnore]
		public new string LogicalNameId { get { return base.LogicalNameId; } }

		[DataMember(Name = "activityadditionalparams")]
        [JsonProperty("activityadditionalparams")]
		public string AdditionalParameters { get { return GetValue<string>("activityadditionalparams"); } set { SetValue("activityadditionalparams", value); } }

		[DataMember(Name = "actualdurationminutes")]
        [JsonProperty("actualdurationminutes")]
		public int? ActualDuration { get { return GetValue<int?>("actualdurationminutes"); } set { SetValue("actualdurationminutes", value); } }

		[DataMember(Name = "actualend")]
        [JsonProperty("actualend")]
		public DateTime ActualEnd { get { return GetValue<DateTime>("actualend"); } set { SetValue("actualend", value); } }

		[DataMember(Name = "actualstart")]
        [JsonProperty("actualstart")]
		public DateTime ActualStart { get { return GetValue<DateTime>("actualstart"); } set { SetValue("actualstart", value); } }

		[DataMember(Name = "attachmentcount")]
        [JsonProperty("attachmentcount")]
		public int? AttachmentCount { get { return GetValue<int?>("attachmentcount"); } set { SetValue("attachmentcount", value); } }

    }
}