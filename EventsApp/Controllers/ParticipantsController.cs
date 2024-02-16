using EventsApp.Models;
using EventsApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IParticipantsService _participantsService;

        public ParticipantsController(IParticipantsService participantsService)
        {
            _participantsService = participantsService;
        }

        [HttpPost]
        public async Task<ActionResult<Participant>> AddParticipant([FromBody] Participant participant)
        {
            var addedParticipant = await _participantsService.AddParticipantAsync(participant);
            return Ok(addedParticipant); // Simplement retourner le participant ajouté
        }


        [HttpGet("{participantId}/events")]
        public async Task<ActionResult> GetParticipantEvents(int participantId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            // Assurez-vous que la méthode de service retourne un tuple avec les événements et le total
            var (events, totalEventsCount) = await _participantsService.GetParticipantEventsAsync(participantId, pageNumber, pageSize);

            // Vous pouvez inclure des informations de pagination dans la réponse si souhaité
            var response = new
            {
                TotalCount = totalEventsCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalEventsCount / pageSize),
                Events = events
            };

            return Ok(response);
        }


    }

}
