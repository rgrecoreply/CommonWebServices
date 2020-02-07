using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using Newtonsoft.Json;
using Services.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CommonWeb.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentController: ApiController
    {
        private readonly IAppointmentService _service;
        private readonly QueryHelper<Appointment> _query = new QueryHelper<Appointment>();

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            IEnumerable<Appointment> appointmnets = _service.GetMultiple(_query.AllColumns());

            return Ok(appointmnets);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IHttpActionResult Get(Guid id)
        {
            Appointment appointmnet = _service.Get(id);

            return Ok(appointmnet);
        }
    }
}