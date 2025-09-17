using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMG_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameDbContext _db;
    private readonly GameService _gameService;
    public GameController(GameDbContext db, GameService gameService)
    {
        _db = db;
        _gameService = gameService;

    }


    [HttpGet("{team_id}")]
    public async Task<ActionResult<IEnumerable<Game>>> GetGames(string team_id)
    {
        try
        {
            var games = await _gameService.GetGamesByTeam(team_id);
            return Ok(games);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpPut("")]
    public async Task<ActionResult<Game>> AddGame([FromBody] GameAddRequest request)
    {

        try
        {
            var game = await _gameService.CreateGame(request);
            return Ok(game);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }

    }
}