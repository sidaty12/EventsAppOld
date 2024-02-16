﻿using EventsApp.Data;
using EventsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Services
{
    public class ParticipantsService : IParticipantsService
    {
        private readonly AppDbContext _context;

        public ParticipantsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Participant> AddParticipantAsync(Participant participant)
        {
            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
            return participant;
        }

        public async Task<(List<Event>, int)> GetParticipantEventsAsync(int participantId, int pageNumber, int pageSize)
        {
            var totalEventsCount = await _context.Registrations
                .CountAsync(r => r.ParticipantId == participantId && !r.IsCanceled);

            var events = await _context.Registrations
                .Where(r => r.ParticipantId == participantId && !r.IsCanceled)
                .Select(r => r.Event)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (events, totalEventsCount);
        }

    }

}
