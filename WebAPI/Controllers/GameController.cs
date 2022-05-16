using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameService gameService;

        public GameController(GameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ICollection<Game>>> GetAllGames()
        {
            var games = await gameService.GetAllGames();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await gameService.GetGameById(id);
            Console.WriteLine(game.ParentGame?.Name);
            return Ok(game);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<int>> AddOrUpdateGame([FromBody] Game game)
        {
            //Console.WriteLine(game.ParentGame.ID + "\n" + game.ParentGame.Name + "\n" + game.PlayersFrom + "\n" + game.PlayersTo + "\n" + game.PlaytimeFrom + "\n" + game.PlaytimeTo + "\n" + game.SubName + "\n" + game.ParentGame);
            var newGame = await gameService.AddOrUpdateGame(game);
            return Ok(newGame.ID);
        }
    }
}
