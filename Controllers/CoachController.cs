using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FMG_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class coachController : ControllerBase
{
    private readonly GameDbContext _db;
    private readonly CoachService _coachService;

    private readonly TeamService _teamService;

    public coachController(GameDbContext db, CoachService coachService, TeamService teamService)
    {
        _db = db;
        _coachService = coachService;
        _teamService = teamService;
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Coach>>> GetAll()
    {
        try
        {
            var coaches = await _coachService.GetAllCoaches();
            return Ok(coaches);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<Coach>>> GetCoachesByTeamId(string teamId)
    {
        try
        {
            var coaches = await _coachService.GetCoachByTeam(teamId);

            return Ok(coaches);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.GetBaseException().Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpGet("{coach_id}")]
    public async Task<ActionResult<Coach>> GetCoachesByIDAsync(string coach_id)
    {
        try
        {
            var coach = await _coachService.GetCoachById(coach_id);
            return Ok(coach);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.GetBaseException().Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpPost("isAffordable")]
    public async Task<ActionResult<bool>> IsCoachAfforable([FromBody] CoachHireRequest request)
    {
        try
        {
            string coachId = request.coachId;
            string teamId = request.teamId;

            bool status = await _coachService.IsCoachAffordableAsync(coachId, teamId);

            return Ok(status);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.GetBaseException().Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpPatch("hire")]
    public async Task<ActionResult<CoachHireResponse>> HireCoach(
        [FromBody] CoachHireRequest request
    )
    {
        try
        {
            string coachId = request.coachId;
            string teamId = request.teamId;

            return Ok(await _coachService.HireCoach(coachId, teamId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.GetBaseException().Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpPatch("fire")]
    public async Task<ActionResult<CoachHireResponse>> FireCoach(
        [FromBody] CoachHireRequest request
    )
    {
        try
        {
            string coachId = request.coachId;
            string teamId = request.teamId;

            return Ok(await _coachService.FireCoach(coachId, teamId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.GetBaseException().Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }

    [HttpGet("teamDetails/{teamId}")]
    public async Task<ActionResult<Team>> GetTeam(string teamId)
    {
        try
        {
            Team team = await _teamService.GetTeamById(teamId);
            return Ok(team);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.GetBaseException().Message);
        }
    }
}
