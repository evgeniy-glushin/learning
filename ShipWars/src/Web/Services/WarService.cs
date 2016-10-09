using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Web.ApplicationContext;
using Web.Enums;
using Web.Models;
using Web.Repositories;
using Web.ViewModels;
using static System.Tuple;

namespace Web.Services
{
    public class WarService
    {
        IRepository<War> _warRepo;
        IActiveWar _activeWar;
        ICurrentUser _currentPlayer;
        UserManager<User> _userManager;
        IObserver<ActiveWarVm> _warObserver;

        public WarService(IActiveWar activeWar,
           ICurrentUser player,
           IRepository<War> warRepo,
           UserManager<User> userManager,
           IObserver<ActiveWarVm> warObserver)
        {
            _activeWar = activeWar;
            _currentPlayer = player;
            _warRepo = warRepo;
            _userManager = userManager;
            _warObserver = warObserver;
        }


        public async Task<Tuple<ActiveWarVm, string>> ShotAsync(ShotVm shot)
        {
            var theWar = _activeWar.TheWar;
            //var theWar = await _warRepo.GetByIdAsync(shot.WarId);

            if (theWar.Id != shot.WarId)
                return Create<ActiveWarVm, string>(null, "Wrong data!");

            if (theWar.WhoShotId != _currentPlayer.UserId)
                return Create<ActiveWarVm, string>(null, "It isn't your turn now!");

            var field = JsonConvert.DeserializeObject<Section[,]>(theWar.Field);

            // Check index bounds.
            if (shot.Row < 0 || shot.Row >= theWar.Rows || shot.Col < 0 || shot.Col >= theWar.Cols)
                return Create<ActiveWarVm, string>(null, "Wrong row or column!");

            //make the shot
            var goalSection = field[shot.Row, shot.Col];
            if (goalSection == null)
            {
                goalSection = new Section(0, SectionState.Damaged);
                field[shot.Row, shot.Col] = goalSection;
            }
            else if (goalSection.State == SectionState.Damaged)
                return Create<ActiveWarVm, string>(null, $"The section [{shot.Row}:{shot.Col}] has already been damaged!");
            else
                goalSection.State = SectionState.Damaged;

            var isMissed = goalSection.Mark == 0;
            var isHit = !isMissed;

            if (isHit && _currentPlayer.PlayerConst == Ship.GetPlayer(goalSection.Mark))
                return Create<ActiveWarVm, string>(null, "You can't hit your own ship!");

            //save the war
            if (isMissed)
                theWar.WhoShotId = _activeWar.CompetitorId;

            switch (_currentPlayer.PlayerConst)
            {
                case Player.Player1:
                    ++theWar.Shots1;
                    if (isHit)
                        ++theWar.Score1;
                    break;
                case Player.Player2:
                    ++theWar.Shots2;
                    if (isHit)
                        ++theWar.Score2;
                    break;
                default:
                    return Create<ActiveWarVm, string>(null, $"There isn't any action for {_currentPlayer.PlayerConst}");
            }

            if (theWar.Score1 == theWar.WinScore || theWar.Score2 == theWar.WinScore)
            {
                theWar.WinnerId = _currentPlayer.UserId;
                theWar.Status = WarStatus.Finished;
                var winner = await _currentPlayer.GetEntity();
                ++winner.Wins;

                var loser = await _userManager.FindByIdAsync(_activeWar.CompetitorId);
                ++loser.Loses;

                await _userManager.UpdateAsync(winner);
                await _userManager.UpdateAsync(loser);
            }

            theWar.Field = JsonConvert.SerializeObject(field);

            _warRepo.Update(theWar);
            await _warRepo.CommitAsync();

            var model = MapWarToActiveWarVm(theWar, _currentPlayer.UserId);
            return Create<ActiveWarVm, string>(model, null);
        }

        public async Task<ActiveWarVm> FindAsync()
        {
            return await FindAsync(_currentPlayer.UserId);
        }
        public async Task<ActiveWarVm> FindAsync(string userId)
        {
            var war = await _warRepo.All
               .Where(w => (w.Player1Id == userId || w.Player2Id == userId))
               .Include(w => w.Player1)
               .Include(w => w.Player2)
               .OrderByDescending(w => w.Id)
               .FirstOrDefaultAsync();

            return MapWarToActiveWarVm(war, userId);
        }

        private ActiveWarVm MapWarToActiveWarVm(War theWar, string userId)
        {
            var model = Mapper.Map<War, ActiveWarVm>(theWar);
            if (model.IsFinished)
                model.GameOverMsg = theWar.WinnerId == userId ? "You win :)" : "You lose :(";
            model.IsShotTime = theWar.WhoShotId == userId;

            var field = JsonConvert.DeserializeObject<Section[,]>(theWar.Field);

            model.Field = GetUserField(field, model, userId);
            return model;
        }

        private Section[,] GetUserField(Section[,] field, ActiveWarVm war, string userId)
        {
            var model = new Section[war.Rows, war.Cols];

            for (int row = 0; row < war.Rows; row++)
                for (int col = 0; col < war.Cols; col++)
                {
                    var section = field[row, col];
                    if (section != null && (section.State == SectionState.Damaged || 
                                            Ship.IsMine(PlayerConst(userId), section.Mark) || 
                                            war.IsFinished))
                    {
                        if (section.State == SectionState.Damaged)
                            section.CssClass += " field-section-damaged ";
                        if (section.Mark != 0)
                            section.CssClass += Ship.GetPlayer(section.Mark) == Player.Player1 ?
                                " field-section-player1-ship " : " field-section-player2-ship ";

                        model[row, col] = section;
                    }
                    else
                        model[row, col] = new Section
                        {
                            CssClass = " field-section ",
                            State = SectionState.Hidden
                        };
                }

            return model;
        }

        private Player PlayerConst(string userId) => 
            _activeWar.TheWar?.Player1Id == userId ? Player.Player1 : Player.Player2;
    }
}
