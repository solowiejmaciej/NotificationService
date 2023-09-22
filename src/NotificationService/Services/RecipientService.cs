using AutoMapper;
using NotificationService.Entities;
using NotificationService.Models;

namespace NotificationService.Services
{
    public interface IRecipientService
    {
        public Task<Recipient> GetRecipientFromUserId(string userId);
    }

    public class RecipientService : IRecipientService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public RecipientService(
            IMapper mapper,
            IUserService userService
            )
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Recipient> GetRecipientFromUserId(string userId)
        {
            var user = await _userService.GetUser(userId);
            var recipient = _mapper.Map<Recipient>(user);
            return recipient;
        }
    }
}