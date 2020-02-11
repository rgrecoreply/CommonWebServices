using CRMWebCommon.EntityWrappers;
using CRMWebCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CommonWebApi.Controllers
{
    public abstract class BaseController<T> : ApiController where T : CrmEntity
    {
        public QueryHelper<T> query;

        public BaseController()
        {
            query = new QueryHelper<T>();
        }
    }
}