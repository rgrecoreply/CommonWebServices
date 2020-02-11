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

        [DataMember(Name = "subject")]
        [JsonProperty("subject")]
        public string Subject { get { return GetValue<string>("subject"); } set { SetValue("subject", value); } }

        [DataMember(Name = "location")]
        [JsonProperty("location")]
        public string Location { get { return GetValue<string>("location"); } set { SetValue("location", value); } }

        [DataMember(Name = "scheduledstart")]
        [JsonProperty("scheduledstart")]
        public DateTime StartTime { get { return GetValue<DateTime>("scheduledstart"); } set { SetValue("scheduledstart", value); } }

        [DataMember(Name = "scheduledend")]
        [JsonProperty("scheduledend")]
        public DateTime EndTime { get { return GetValue<DateTime>("scheduledend"); } set { SetValue("scheduledend", value); } }

    }
}