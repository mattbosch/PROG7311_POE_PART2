using POETEST_MVC.Models;
using POETEST_MVC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POETEST_MVC.Services.Business
{
    public class SecurityService
    {
        SecurityDAO daoService = new SecurityDAO();

        public bool Authenticate(User user)
        {
            return daoService.FindUser(user, out string role);
        }
    }
}