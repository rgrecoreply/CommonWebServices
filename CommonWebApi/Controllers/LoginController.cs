using PersistenceContext.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace CommonWebApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class LoginController: ApiController
    {
        [HttpPost]
        [Route("form")]
        public IHttpActionResult Login([FromBody]string username, [FromBody]string password)
        {
            IPersistenceContext context = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPersistenceContext)) as IPersistenceContext;
            if (context.GetType().Equals(typeof(ICrmContext)))
            {
                if (((ICrmContext)context).GetService(username, password) != null)
                    return Ok();
                else
                    return Unauthorized();
            }

            return Ok();
        }

        [HttpPost]
        [Route("user")]
        public IHttpActionResult LoginUserId([FromBody]string userId)
        {
            IPersistenceContext context = GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPersistenceContext)) as IPersistenceContext;
            if (context.GetType().Equals(typeof(ICrmContext)))
            {
                if (((ICrmContext)context).GetService(userId) != null)
                    return Ok();
                else
                    return Unauthorized();
            }

            return Ok();
        }
    }
}