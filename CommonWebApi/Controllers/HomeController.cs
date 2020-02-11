using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using Microsoft.Xrm.Sdk.Query;
using Services.EntityInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CommonWebApi.Controllers
{
    public class HomeController : Controller
    {
        private IAppointmentService _service;
        private QueryHelper<Appointment> _query = new QueryHelper<Appointment>();

        public HomeController()
        {
            _service = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IAppointmentService)) as IAppointmentService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Table()
        {
            ViewBag.Title = "Home Page";
            ViewBag.Appointments = _service.GetMultiple(_query
                .AddCondition(x => x.Subject, ConditionOperator.Like, "%test%")
                .AddColumns(x => x.Subject, x => x.Location, x => x.StartTime, x => x.EndTime));

            return View();
        }
    }
}
