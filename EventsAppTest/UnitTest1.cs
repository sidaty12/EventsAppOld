using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit; // Assurez-vous d'avoir xUnit install� et utilis�
using EventsApp.Data;
using Microsoft.EntityFrameworkCore;
using EventsApp.Models;
using EventsApp.Services;

namespace EventsAppTest
{
    public class EventsServiceTests
    {
        [Fact]
        public async Task GetParticipantEventsAsync_ReturnsCorrectData()
        {
            // Configuration pour utiliser une base de donn�es InMemory
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb") // Assurez-vous que le nom de la base de donn�es est unique pour chaque test
                .Options;

            // Initialisation du contexte avec la base de donn�es InMemory
            using (var context = new AppDbContext(options))
            {
                var expectedParticipantId = 1;
                var expectedPageNumber = 1;
                var expectedPageSize = 2;

                // Initialisation et ajout des donn�es dans la base de donn�es InMemory
                var event1 = new Event { Id = 1, Title = "Event 1", Location = "Location 1" };
                var event2 = new Event { Id = 2, Title = "Event 2", Location = "Location 2" };

                context.Events.AddRange(event1, event2);
                context.Registrations.AddRange(
                    new Registration { ParticipantId = expectedParticipantId, Event = event1, IsCanceled = false },
                    new Registration { ParticipantId = expectedParticipantId, Event = event2, IsCanceled = false }
                );
                context.SaveChanges();

                // Cr�ation du service � tester avec le contexte InMemory
                var service = new ParticipantsService(context); // Assurez-vous que le nom du service est correct

                // Ex�cution de la m�thode � tester
                var (events, totalEventsCount) = await service.GetParticipantEventsAsync(expectedParticipantId, expectedPageNumber, expectedPageSize);

                // Assertions pour v�rifier le comportement attendu
                Assert.Equal(expectedPageSize, events.Count); // V�rifie que le nombre d'�v�nements retourn�s correspond � la taille de page attendue
                Assert.True(totalEventsCount >= events.Count); // V�rifie que le nombre total d'�v�nements est sup�rieur ou �gal au nombre d'�v�nements retourn�s
            }
        }
    }
}
