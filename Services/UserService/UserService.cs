using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TermProject.models;

namespace TermProject.Services.UserService;

public class UserService : IUserService
{
    private readonly UserManager<MyUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<MyUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserVm> GetInformation(string userId)
    {
        var currentUser = await _userManager.FindByIdAsync(userId);

        var currentUserVm = _mapper.Map<MyUser, UserVm>(currentUser);

        return currentUserVm;
    }
}