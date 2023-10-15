using EventsConsumer.Models.Entity;
using EventsConsumer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventsConsumer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventsRepository _eventsRepository;

    public EventsController(
        IEventsRepository eventsRepository
        )
    {
        _eventsRepository = eventsRepository;
    }

    [HttpGet]
    public async Task <ActionResult> GetAllAsync()
    {
        var test = await _eventsRepository.GetAll();
        return Ok(test);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetByIdAsync(Guid id)
    {
        var test = await _eventsRepository.GetByIdAsync(id);
        return Ok(test);
    }
}