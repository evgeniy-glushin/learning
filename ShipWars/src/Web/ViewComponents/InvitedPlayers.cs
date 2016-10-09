using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Repositories;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class InvitedPlayers : ViewComponent
    {
        private IRepository<Invitation> _invitationRepo;
        private UserManager<User> _userManager;

        public InvitedPlayers(UserManager<User> userManager,
            IRepository<Invitation> invitationRepo)
        {
            _userManager = userManager;
            _invitationRepo = invitationRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentPlayer = await _userManager.FindByNameAsync(User.Identity.Name);

            var inviterId = currentPlayer.Id;

            var model = await _invitationRepo.All
                .Where(i => i.InviterId == inviterId)
                .Select(i => new InvitedPlayerVm
                {
                    InvitationId = i.Id,
                    Nickname = i.Invited.Nickname,
                    Wins = i.Invited.Wins,
                    Loses = i.Invited.Loses
                }).ToListAsync();

            return View("Default", model);
        }
    }
}
