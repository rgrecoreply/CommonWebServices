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
    public class ResponseError<T>: ResponseBase<T>
    {
        [DataMember]
        [JsonProperty("stringError")]
        public string StringError { get; set; }

        public ResponseError() 
        {
            Code = 500;
        }

        public ResponseError(string stringError)
        {
            Code = 500;
            StringError = stringError;
        }

        public ResponseError(int code, string stringError)
        {
            Code = code;
            StringError = stringError;
        }
    }
}
