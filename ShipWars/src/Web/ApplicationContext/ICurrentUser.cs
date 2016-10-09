using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Enums;
using Web.Models;

namespace Web.ApplicationContext
{
    public interface ICurrentUser
    {
        Task<User> GetEntity();
        string UserId { get; }

        Player PlayerConst { get; }
    }
}
