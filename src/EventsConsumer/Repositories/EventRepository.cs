#region

using EventsConsumer.Models.Entity;
using EventsConsumer.Persistant;
using Microsoft.EntityFrameworkCore;

#endregion

namespace EventsConsumer.Repositories;

public interface IEventsRepository
{
    public Task AddAsync(Event @event);
    public Task<Event?> GetByIdAsync(Guid id);
    public Task ChangeStatusAsync(Event @event);
    public Task<List<Event>> GetAll();
    Task<bool> UpdateAsync(Event baseEvent);
}

internal class EventsRepository : IEventsRepository
{
    private readonly EventsDbContext _eventsDbContext;

    public EventsRepository(EventsDbContext eventsDbContext)
    {
        _eventsDbContext = eventsDbContext;
    }

    public async Task AddAsync(Event @event)
    {
        await _eventsDbContext.Events.AddAsync(@event);
        await _eventsDbContext.SaveChangesAsync();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _eventsDbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task ChangeStatusAsync(Event @event)
    {
        var eventToUpdate = await GetByIdAsync(@event.Id);
        if (eventToUpdate == null) throw new Exception("Event not found");

        eventToUpdate.Status = @event.Status;
        await _eventsDbContext.SaveChangesAsync();
    }

    public async Task<List<Event>> GetAll()
    {
        return await _eventsDbContext.Events.ToListAsync();
    }

    public async Task<bool> UpdateAsync(Event baseEvent)
    {
        var eventToUpdate = await GetByIdAsync(baseEvent.Id);
        if (eventToUpdate == null) throw new Exception("Event not found");

        eventToUpdate.ErrorMessage = baseEvent.ErrorMessage;
        await _eventsDbContext.SaveChangesAsync();

        return true;
    }
}