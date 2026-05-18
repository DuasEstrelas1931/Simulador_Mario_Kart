
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulador_Mario_Kart.Data;
using Simulador_Mario_Kart.DTOs;
using Simulador_Mario_Kart.Models;
using Simulador_Mario_Kart.Services;

namespace MarioKartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class RaceController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly RaceEngineService _engine;

        public RaceController(AppDbContext db, RaceEngineService engine)
        {
            _db = db;
            _engine = engine;
        }

        /// <summary>Lista todos os personagens disponíveis com seus atributos.</summary>
        [HttpGet("characters")]
        [AllowAnonymous]
        public IActionResult GetCharacters() =>
            Ok(RaceEngineService.GetAvailableCharacters());

        /// <summary>
        /// Inicia uma corrida completa e retorna o histórico lap a lap + resultado final.
        /// Salva automaticamente os resultados no banco.
        /// </summary>
        [HttpPost("start")]
        [ProducesResponseType(typeof(RaceFinishDto), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> StartRace([FromBody] StartRaceRequest req)
        {
            if (req.Players.Count < 2)
                return BadRequest(new { message = "Mínimo 2 jogadores." });

            // Validate users exist
            var userIds = req.Players.Select(p => p.UserId).ToList();
            var users = await _db.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id);

            foreach (var p in req.Players)
                if (!users.ContainsKey(p.UserId))
                    return BadRequest(new { message = $"Usuário ID {p.UserId} não encontrado." });

            // Create race record
            var race = new Race
            {
                TrackName = req.TrackName,
                TotalLaps = req.TotalLaps,
                Status = Simulador_Mario_Kart.Models.RaceStatus.Running,
            };
            _db.Races.Add(race);
            await _db.SaveChangesAsync();

            // Simulate
            var simulation = _engine.SimulateRace(req);

            // Save results
            foreach (var state in simulation.PlayerStates)
            {
                var result = new RaceResult
                {
                    RaceId = race.Id,
                    UserId = state.UserId,
                    CharacterName = state.CharacterName,
                    CharacterEmoji = state.CharacterEmoji,
                    FinalPosition = state.FinalPosition,
                    TotalPoints = state.TotalPoints,
                    Wins = state.Wins,
                    Losses = state.Losses,
                    TotalTime = state.TotalTime,
                };
                _db.RaceResults.Add(result);
            }

            race.Status = Simulador_Mario_Kart.Models.RaceStatus.Finished;
            race.FinishedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            // Build response
            var finalStandingsWithUsernames = simulation.FinalStandings.Select(s =>
                s with { Username = users[s.UserId].Username }
            ).ToList();

            var winner = finalStandingsWithUsernames.First();

            var response = new RaceFinishDto(
                race.Id,
                race.TrackName,
                winner.CharacterName,
                winner.CharacterEmoji,
                winner.UserId,
                finalStandingsWithUsernames
            );

            // Return full simulation history + final result
            return Ok(new
            {
                Race = response,
                LapHistory = simulation.LapHistory
            });
        }

        /// <summary>Retorna os resultados de uma corrida específica.</summary>
        [HttpGet("{raceId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRace(int raceId)
        {
            var race = await _db.Races
                .Include(r => r.Results)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == raceId);

            if (race is null)
                return NotFound();

            return Ok(new
            {
                race.Id,
                race.TrackName,
                race.TotalLaps,
                race.Status,
                race.StartedAt,
                race.FinishedAt,
                Results = race.Results
                    .OrderBy(r => r.FinalPosition)
                    .Select(r => new
                    {
                        r.FinalPosition,
                        r.CharacterName,
                        r.CharacterEmoji,
                        r.User.Username,
                        r.TotalPoints,
                        r.Wins,
                        r.Losses,
                        r.TotalTime
                    })
            });
        }
    }
}