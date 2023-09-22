using System.Threading;
using System.Threading.Tasks;
using AuthService.Application.Dtos;
using AuthService.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Exceptions;

namespace AuthService.Application.MediatR.Query
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUsersRepository _repository;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(
            IUsersRepository repository,
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (user is null)
            {
                throw new NotFoundException($"User with id {request.Id} not found");
            }
            var dto = _mapper.Map<UserDto>(user);
            return dto;
        }
        
    }
    
    public record GetUserByIdQuery : IRequest<UserDto>
    {
        public string Id { get; set; }
    }
}