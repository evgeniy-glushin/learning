using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Web.ApplicationContext;
using Web.Enums;
using Web.Models;
using Web.Repositories;
using Web.ViewModels;

namespace Web.ViewComponents
{
    public class MyInvitations : ViewComponent
    {
        private IRepository<Invitation> _invitationRepo;
        private ICurrentUser _player;

        public MyInvitations(IRepository<Invitation> invitationRepo,
            ICurrentUser player)
        {
            _invitationRepo = invitationRepo;
            _player = player;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var invitedPlayer = await _player.GetEntity();
            var model = await _invitationRepo.All
                .Where(i => i.InvitedId == invitedPlayer.Id &&
                            i.Status == InvitationStatus.Active)
                .ProjectTo<MyInvitationVm>()               
                .ToListAsync();

            return View(model);
        }
    }
}
