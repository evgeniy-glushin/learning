using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web.Data;
using Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Web.Models;
using Web.Repositories;
using AutoMapper;
using Web.ApplicationContext;

namespace Web.ViewComponents
{
    public class OnlinePlayers : ViewComponent
    {
        private IRepository<Invitation> _invitationRepo;
        private UserManager<User> _userManager;
        private ICurrentUser _player;

        public OnlinePlayers(ICurrentUser player,
            UserManager<User> userManager,
            IRepository<Invitation> invitationRepo)
        {
            _player = player;
            _userManager = userManager;
            _invitationRepo = invitationRepo;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentPlayer = await _player.GetEntity();
            var model = await _userManager.Users
                .Where(u => u.IsOnline &&
                            u.Id != currentPlayer.Id &&
                            !_invitationRepo.All.Any(i => (i.InvitedId == currentPlayer.Id ||
                                                          i.InviterId == currentPlayer.Id) &&
                                                          i.Status == Enums.InvitationStatus.Active))            
                .ToListAsync();

            return View(Mapper.Map<IEnumerable<OnlinePlayerVm>>(model));
        }
    }
}
