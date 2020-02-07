using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommonWebApi.Responses
{
    public class GetAllAppointmentResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("name")]
        public int Name { get; set; }
    }
}