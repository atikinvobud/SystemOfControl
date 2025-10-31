using BackEnd.DTOs;
using BackEnd.Share;

namespace BackEnd.Services;

public interface IAuthorizationService
{
    Task<Result<Guid?>> RegistrUser(RegistrDTO registrDTO);
    Task<Result<AnswerLoginDTO>> Login(LoginDTO loginDTO);
    Task<bool> ChangePassword(string login, string newPassword);
}