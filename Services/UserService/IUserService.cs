using TermProject.models;

namespace TermProject.Services.UserService;

public interface IUserService
{
    Task<UserVm> GetInformation(string userId);
}