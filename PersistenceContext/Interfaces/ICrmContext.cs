using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceContext.Interfaces
{
    public interface ICrmContext
    {
        IOrganizationService GetService();
        IOrganizationService GetService(string connectionString);
        IOrganizationService GetCrmServiceByConnectionString(string connectionString);
    }
}
