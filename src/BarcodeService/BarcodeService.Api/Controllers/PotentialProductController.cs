using BarcodeService.Api.Models.Requests;
using BarcodeService.Application.MediatR.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarcodeService.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PotentialProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public PotentialProductController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<IActionResult> AddNewPotentialProduct(
        [FromBody] AddNewPotentialProductRequest request
    )
    {
        var command = new AddNewPotentialProductCommand
        {
            Ean = request.Ean,
            Price = request.Price,
            Name = request.Name
        }; 
        await _mediator.Send(command);
        return Ok();
    }
}