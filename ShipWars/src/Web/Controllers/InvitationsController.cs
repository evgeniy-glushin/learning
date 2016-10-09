using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web.ApplicationContext;
using Web.Enums;
using Web.Models;
using Web.Repositories;
using Web.Services;
using Newtonsoft.Json;
using static Web.Enums.NotificationType;

namespace Web.Controllers
{
    public class InvitationsController : Controller
    {
        IRepository<Invitation> _invitationRepo;
        UserManager<User> _userManager;
        ICurrentUser _player;
        IRepository<War> _warRepo;
        IActiveWar _activeWar;
        FieldBuilder _fieldBuilder;
        GameConfig _config;
        private NotificationsService _notifServ;

        public InvitationsController(UserManager<User> userManager,
            IRepository<Invitation> invitationRepo,
            IRepository<War> warRepo,
            ICurrentUser player,
            IActiveWar activeWar,
            FieldBuilder fieldBuilder,
            GameConfig config,
            NotificationsService notifServ)
        {
            _userManager = userManager;
            _invitationRepo = invitationRepo;
            _warRepo = warRepo;
            _player = player;
            _activeWar = activeWar;
            _fieldBuilder = fieldBuilder;
            _config = config;
            _notifServ = notifServ;
        }

        [HttpPost]
        public async Task<IActionResult> Accept(int id)
        {
            if (_activeWar.TheWar != null) return BadRequest("You can have only one active war.");

            var invitedPlayer = await _player.GetEntity();
            var invitation = await _invitationRepo.GetByIdAsync(id);

            if (invitation == null)
                return NotFound(id);

            var newWar = new War
            {
                Player1Id = invitation.InviterId,
                Player2Id = invitation.InvitedId,
                WhoShotId = invitation.InvitedId,
                Field = JsonConvert.SerializeObject(_fieldBuilder.Build()),
                Status = WarStatus.Active,
                Rows = _config.Rows,
                Cols = _config.Cols,
                WinScore = CalcWinScore()
            };

            _warRepo.Insert(newWar);
            _invitationRepo.Delete(invitation);
            await _warRepo.CommitAsync();

            _notifServ.Add(new Notification(invitation.InviterId, InvitationAccepted));

            return RedirectToAction("Index", "Home");
        }
        int CalcWinScore() => (_config.OnePieceShipsCount + _config.TwoPieceShipsCount * 2 + _config.ThreePieceShipsCount * 3) / 2;

        [HttpPost]
        public async Task<IActionResult> Invite(string id)
        {
            var invitedPlayer = await _userManager.FindByIdAsync(id);
            if (invitedPlayer != null)
            {
                var inviterId = _userManager.GetUserId(User);
                var isInvitationAlreadyExists = await _invitationRepo.All.AnyAsync(i => ((i.InviterId == inviterId && i.InvitedId == invitedPlayer.Id) ||
                                                                                         (i.InvitedId == inviterId && i.InviterId == invitedPlayer.Id)) &&
                                                                                          i.Status == InvitationStatus.Active);
                var isSelfInvitation = inviterId == invitedPlayer.Id;

                if (!isInvitationAlreadyExists && !isSelfInvitation && invitedPlayer.IsOnline)
                {
                    var newInvitation = new Invitation
                    {
                        InvitedId = invitedPlayer.Id,
                        InviterId = inviterId,
                        Seen = false,
                        Status = InvitationStatus.Active
                    };
                    _invitationRepo.Insert(newInvitation);
                    await _invitationRepo.CommitAsync();

                    _notifServ.Add(new Notification(newInvitation.InvitedId, InvitationCreated));
                }
            }
            else
                return NotFound(id);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var inviterId = _userManager.GetUserId(User);
            var invitation = await _invitationRepo.GetByIdAsync(id);
            if (invitation != null)
            {
                _invitationRepo.Delete(invitation);
                await _invitationRepo.CommitAsync();
                _notifServ.Add(new Notification(invitation.InvitedId, InvitationDeleted));
            }
            else
                return NotFound();

            return RedirectToAction("Index", "Home");
        }
    }
}
