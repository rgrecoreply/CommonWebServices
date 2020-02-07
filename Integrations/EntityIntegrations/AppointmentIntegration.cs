using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using Services.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integrations.EntityIntegrations
{
    public class AppointmentIntegration
    {
        private readonly IAppointmentService _service;

        public AppointmentIntegration(IAppointmentService service)
        {
            _service = service;
        }

        public IEnumerable<Appointment> GetAll()
        {
            IEnumerable<string> cols = new Appointment();
        }
    }
}
