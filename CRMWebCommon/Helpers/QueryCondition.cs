using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMWebCommon.Helpers
{
    public static class QueryCondition
    {
        public static ConditionOperator? Get(string query)
        {
            if (query.IndexOf('=') > -1) return ConditionOperator.Equal;
            else if (query.IndexOf("!=") > -1) return ConditionOperator.NotEqual;
            else if (query.IndexOf('>') > -1) return ConditionOperator.GreaterThan;
            else if (query.IndexOf(">=") > -1) return ConditionOperator.GreaterEqual;
            else if (query.IndexOf('<') > -1) return ConditionOperator.LessThan;
            else if (query.IndexOf("<=") > -1) return ConditionOperator.LessEqual;
            else if (query.IndexOf("!!") > -1) return ConditionOperator.NotNull;
            else return null;
        }
    }
}
