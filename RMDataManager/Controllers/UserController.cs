using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace RMDataManager.Controllers
{
    [System.Web.Http.Authorize]
    public class UserController : ApiController
    {
     
        public UserModel GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();
            
            return data.GetUserById(id).First();
        }

    }
}
