using EventsConsumer.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace EventsConsumer.Persistant
{
    internal class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}