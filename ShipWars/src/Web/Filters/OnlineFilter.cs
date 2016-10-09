using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data;
using Web.Models;

namespace Web.Filters
{
    public class OnlineFilter : IActionFilter
    {
        private ShipWarsDbContext _uow;
       
        //public OnlineFilter(ShipWarsDbContext uow)
        //{
        //    _uow = uow;
        //}

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            //var user = await _uow.Users.FirstOrDefaultAsync(u => u.UserName == context.HttpContext.User.Identity.Name);
            //if (user != null && !user.IsOnline)
            //{
            //    user.IsOnline = true;
            //    _uow.SaveChanges();
            //}
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

    }
}
