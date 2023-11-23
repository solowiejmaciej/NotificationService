#region

using AuthService.Application.ApplicationUserContext;
using AuthService.Application.Dtos;
using AuthService.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;

#endregion

namespace AuthService.Application.MediatR.Command;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public UpdateUserCommandHandler(
        IUsersRepository repository,
        IMapper mapper,
        IUserContext userContext
    )
    {
        _repository = repository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null) throw new NotFoundException($"User with id {request.Id} not found");

        var currentUser = _userContext.GetCurrentUser();
        //var isAdmin = currentUser.Role == "Admin";
        var isAdmin = false;
        if (!isAdmin && currentUser.Id != request.Id)
            throw new AccessForbiddenException($"User {currentUser.Id} tried to edit {request.Id} user ");

        if (!request.Firstname.IsNullOrEmpty()) user.Firstname = request.Firstname;
        if (!request.Firstname.IsNullOrEmpty()) ;
        {
            user.Firstname = request.Firstname;
        }

        if (!request.Surname.IsNullOrEmpty()) user.Surname = request.Surname;

        if (!request.Email.IsNullOrEmpty()) user.Email = request.Email;

        if (!request.PhoneNumber.IsNullOrEmpty()) user.PhoneNumber = request.PhoneNumber;
        await _repository.SaveAsync(cancellationToken);
        var dto = _mapper.Map<UserDto>(user);
        return dto;
    }
}

public record UpdateUserCommand : IRequest<UserDto>
{
    public string Id { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? DeviceId { get; set; }
    public string Firstname { get; set; }
    public string Surname { get; set; }
}