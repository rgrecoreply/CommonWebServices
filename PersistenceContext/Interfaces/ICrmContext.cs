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
        IOrganizationService GetService(string userId);
        IOrganizationService GetService(string username, string password);
        IOrganizationService GetServiceByConnectionStringName(string connectionStringName);
        IOrganizationService GetCrmServiceByConnectionString(string connectionString);
    }
}
