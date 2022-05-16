using Domain.Models;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class GameService
    {
        private readonly IGameRepo gameRepo;

        public GameService(IGameRepo gameRepo)
        {
            this.gameRepo = gameRepo;
        }

        public async Task<ICollection<Game>> GetAllGames()
        {
            return await gameRepo.List();
        }

        public async Task<Game> GetGameById(int id)
        {
            return await gameRepo.FindById(id);
        }

        public async Task<Game> AddOrUpdateGame(Game game)
        {
            var newGame = await gameRepo.Insert(game);
            return newGame;
        }
    }
}
