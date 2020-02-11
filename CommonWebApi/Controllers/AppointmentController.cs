using CommonWebApi.Models.Binder;
using CommonWebApi.Responses;
using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using Services.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;

namespace CommonWebApi.Controllers
{
    [RoutePrefix("api/appointments")]
    public class AppointmentController: BaseController<Appointment>
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service): base()
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        [Route("{filters?}/{columns?}")]
        public IHttpActionResult Get(string filters = "", string columns = "")
        {
            if (String.IsNullOrEmpty(filters) && String.IsNullOrEmpty(columns))
                return Ok(_service.GetAll().Select(x => new GetAllAppointmentResponse()
                {
                    Id = x.Id,
                    Parent = x.AdditionalParameters,
                    Name = (int)(x.AttachmentCount ?? 0)
                }));
            else if (String.IsNullOrEmpty(columns))
            {
                foreach (Filter filterObj in FilterModelBinder.BindModel(filters))
                    if (String.IsNullOrEmpty(filterObj.FilterValue))
                        query.AddCondition(filterObj.FilterName, QueryCondition.Get(filterObj.FilterOperator));
                    else
                        query.AddCondition(filterObj.FilterName, QueryCondition.Get(filterObj.FilterOperator), filterObj.FilterValue);

                return Ok(_service.GetMultiple(query.AllColumns()));
            }
            else if (String.IsNullOrEmpty(filters))
            {
                return Ok(_service.GetMultiple(query.AddColumns(columns.Split(','))));
            }
            else
            {
                foreach (Filter filterObj in FilterModelBinder.BindModel(filters))
                    if (String.IsNullOrEmpty(filterObj.FilterValue))
                        query.AddCondition(filterObj.FilterName, QueryCondition.Get(filterObj.FilterOperator));
                    else
                        query.AddCondition(filterObj.FilterName, QueryCondition.Get(filterObj.FilterOperator), filterObj.FilterValue);

                return Ok(_service.GetMultiple(query.AddColumns(columns.Split(','))));
            }

        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetId(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return BadRequest("id not set properly");

            Appointment appointmnet = _service.Get(guid);

            return Ok(appointmnet);
        }

        [HttpPatch]
        [Route("")]
        public void UpdateCache([FromBody]Appointment appointment)
        {
            _service.Update(appointment);
        }
    }
}