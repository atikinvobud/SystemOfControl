using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using BackEnd.DTOs;
using BackEnd.Extensions;
using BackEnd.Models;
using BackEnd.Models.Entities;
using BackEnd.Repositories;
using BackEnd.Share;
using FluentValidation;


namespace BackEnd.Services;

public class AuthorizationServic : IAuthorizationServic
{
    private readonly IUserRepository userRepository;
    private readonly IHashService hashService;
    private readonly AbstractValidator<RegistrDTO> validator;
    private readonly IJwtService jwtService;
    private readonly IRefreshTokenService refreshTokenService;
    private readonly ICoockieService coockieService;
    public AuthorizationServic(IUserRepository userRepository, IHashService hashService, AbstractValidator<RegistrDTO> validator, IJwtService jwtService, IRefreshTokenService refreshTokenService, ICoockieService coockieService)
    {
        this.userRepository = userRepository;
        this.hashService = hashService;
        this.validator = validator;
        this.jwtService = jwtService;
        this.refreshTokenService = refreshTokenService;
        this.coockieService = coockieService;
    }
    public async Task<bool> ChangePassword(string login, string newPassword)
    {
        User? user = await userRepository.GetUserByLogin(login);
        if (user == null) return false;
        newPassword = hashService.Hash(newPassword);
        await userRepository.UpdateUser(user, newPassword);
        return  true;
    }

    public async Task<Result<AnswerLoginDTO>> Login(LoginDTO loginDTO)
    {
        User? user = await userRepository.GetUserByLogin(loginDTO.Login);
        if (user == null) return Result<AnswerLoginDTO>.Error(ErrorCode.UserNotFound);
        if (!hashService.Verify(user!.HashPassword, loginDTO.Password)) return Result<AnswerLoginDTO>.Error(ErrorCode.WrongPassword);

        List<string> roles = user.GetRoles();
        string token = jwtService.GenerateToken(user, roles);
        string coockieName = jwtService.GetCoockieName();
        int expireHours = jwtService.GetExpireHours();

        string refreshToken = await refreshTokenService.CreateToken(user);
        bool flag = coockieService.SetCookie(coockieName, refreshToken, 10);
        if (!flag) return Result<AnswerLoginDTO>.Error(ErrorCode.CoockieError);
        AnswerLoginDTO answer = new AnswerLoginDTO().With(x => x.UserId = user.Id)
            .With(x => x.Roles = roles).With(x => x.Token = token).With(x => x.CoockieName = coockieName).With(x => x.ExpireMinutes = expireHours);
        return Result<AnswerLoginDTO>.Success(answer);
    }

    public async Task<Result<bool>> Logout(Guid id)
    {
        string coockiename = jwtService.GetCoockieName();
        string? token = coockieService.GetCookie(coockiename);
        if (token == null) return Result<bool>.Error(ErrorCode.NotFoundToken);
        bool flag = await refreshTokenService.DeleteToken(token, id);
        if (!flag) return Result<bool>.Error(ErrorCode.DeleteTokenError);
        coockieService.DeleteCookie(coockiename);
        return Result<bool>.Success(true);
    }

    public async Task<Result<ChangeTokenDTO>> RefreshToken(Guid id)
    {
        string coockiename = jwtService.GetCoockieName();
        string? token = coockieService.GetCookie(coockiename);
        if (token == null) return Result<ChangeTokenDTO>.Error(ErrorCode.NotFoundToken);
        bool flag = await refreshTokenService.IsTokenValid(token, id);
        if (!flag) return Result<ChangeTokenDTO>.Error(ErrorCode.InvalidRefreshToken);
        User? userEntity = await userRepository.GetUserById(id);
        if (userEntity == null) return Result<ChangeTokenDTO>.Error(ErrorCode.UserNotFound);
        List<string> roles = userEntity.GetRoles();
        string newRefreshToken = await refreshTokenService.CreateToken(userEntity);
        string newAccessToken = jwtService.GenerateToken(userEntity, roles);
        flag = coockieService.SetCookie(coockiename, newRefreshToken, 10);
        if (!flag) return Result<ChangeTokenDTO>.Error(ErrorCode.CoockieError);
        return Result<ChangeTokenDTO>.Success(new ChangeTokenDTO{AccessToken = newAccessToken});
    }

    public async Task<Result<Guid?>> RegistrUser(RegistrDTO registrDTO)
    {
        List<User> users = await userRepository.GetAllUsers();
        if (users.Where(u => u.Login == registrDTO.Login).Count() == 0)
        {
            var result = await validator.ValidateAsync(registrDTO);
            if (!result.IsValid)
            {
                var firstError = result.Errors.First();
                var code = Enum.Parse<ErrorCode>(firstError.ErrorCode);
                return Result<Guid?>.Error(code);
            }
            registrDTO.Password = hashService.Hash(registrDTO.Password);
            User user = registrDTO.ToEntity();
            UserInfo userInfo = registrDTO.ToUserInfo(user.Id);
            Guid? id = await userRepository.CreateUser(user, userInfo);
            if (id is null) return Result<Guid?>.Error(ErrorCode.UserCreationError);
            return Result<Guid?>.Success(id);
        }
        return Result<Guid?>.Error(ErrorCode.RepeatLogin);
    }
}