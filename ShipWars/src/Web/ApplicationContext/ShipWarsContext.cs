using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Web.Enums;
using Web.Models;
using Web.Repositories;
using System;
using Microsoft.EntityFrameworkCore;
using Web.Services;

namespace Web.ApplicationContext
{
    public class ShipWarsContext : ICurrentUser, IActiveWar
    {
        IHttpContextAccessor _contextAccessor;
        UserManager<User> _userManager;
        IRepository<War> _warRepo;
        GameConfig _config;

        public ShipWarsContext(IHttpContextAccessor contextAccessor,
            UserManager<User> userManager,
            IRepository<War> warRepo,
            GameConfig config)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _warRepo = warRepo;
            _config = config;
        }

        public string UserId =>
            _userManager.GetUserId(_contextAccessor.HttpContext.User);

        War _theWar;
        public War TheWar =>
            _theWar ?? (_theWar = _warRepo.All
                .Include(w => w.Player1)
                .Include(w => w.Player2)
                .FirstOrDefault(w => (w.Player1Id == UserId || w.Player2Id == UserId) &&
                                      w.Status == WarStatus.Active));

        //public War TheWar =>
        //   _warRepo.All
        //        .Include(w => w.Player1)
        //        .Include(w => w.Player2)
        //        .FirstOrDefault(w => (w.Player1Id == UserId || w.Player2Id == UserId) &&
        //                              w.Status == WarStatus.Active);

        public bool Exists =>
            _warRepo.All.Any(w => (w.Player1Id == UserId || w.Player2Id == UserId) &&
                                   w.Status == WarStatus.Active);

        public Player PlayerConst =>
            TheWar?.Player1Id == UserId ? Player.Player1 : Player.Player2;

        public string CompetitorId =>
            TheWar.Player1Id == UserId ? TheWar.Player2Id : TheWar.Player1Id;


        public async Task<User> GetEntity() =>
            await _userManager.FindByNameAsync(_contextAccessor.HttpContext.User.Identity.Name);

    }
}
