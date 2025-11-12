using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IAuthorizationServic
{
    Task<Result<Guid?>> RegistrUser(RegistrDTO registrDTO);
    Task<Result<AnswerLoginDTO>> Login(LoginDTO loginDTO);
    Task<Result<bool>> Logout(Guid id);
    Task<Result<ChangeTokenDTO>> RefreshToken(Guid id);
    Task<bool> ChangePassword(string login, string newPassword);
}