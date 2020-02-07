using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using PersistenceContext.Interfaces;
using Services.EntityInterfaces;
using Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EntityServices
{
    public class AppointmentService : ServiceBase<Appointment>, IAppointmentService
    {
        public AppointmentService(IPersistenceContext context) : base(context) { }

    }
}
