using BackEnd.DTOs;
using BackEnd.Extensions;
using BackEnd.Models.Entities;
using BackEnd.Repositories;
using BackEnd.Share;

namespace BackEnd.Services;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IUserInfoRepository userInfoRepository;
    public UserService(IUserRepository userRepository, IUserInfoRepository userInfoRepository)
    {
        this.userRepository = userRepository;
        this.userInfoRepository = userInfoRepository;
    }

    public async Task<Result<GetUser>> GetUser(Guid Id)
    {
        User? user = await userRepository.GetUserById(Id);
        if (user is null) Result<GetUser>.Error(ErrorCode.UserNotFound);
        return Result<GetUser>.Success(user!.ToDTO());
    }

    public async Task<Result<bool>> UpdateUserInfo(PutUserInfo putUserInfo)
    {
        UserInfo? userInfo = await userInfoRepository.GetUserInfo(putUserInfo.Id);
        if (userInfo is null) Result<bool>.Error(ErrorCode.UserInfoNotFound);
        userInfo!.UpdateUserInfo(putUserInfo);
        await userInfoRepository.UpdateUserInfo(userInfo!);
        return Result<bool>.Success(true);
    }
    public async Task<Result<Guid>> AppointRole(PostUserRole postUserRole)
    {
        UserRole userRole = postUserRole.ToEntity();
        Guid Id = await userRepository.AppointRole(userRole);
        return Result<Guid>.Success(Id);
    }

    public async Task<Result<List<GetUser>>> GetUsersWithPagination(string? role, int page, int pageSize)
    {
        List<User> users = await userRepository.GelAllWithPagination(role, page, pageSize);
        List<GetUser> dtos = new List<GetUser>();
        foreach (var user in users)
        {
            dtos.Add(user.ToDTO());
        }
        return Result<List<GetUser>>.Success(dtos);
    }
}