using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace OauthDemo.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]
        public string Get()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}
