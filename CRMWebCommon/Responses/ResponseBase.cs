using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebCommon.Responses
{
    [DataContract]
    public class ResponseBase<T>
    {
        [DataMember]
        [JsonProperty("code")]
        public int Code { get; set; }

        public ResponseBase() {
            Code = 200;
        }

        public ResponseBase(int code)
        {
            Code = code;
        }
    }
}
