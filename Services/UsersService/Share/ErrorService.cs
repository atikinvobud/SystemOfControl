using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BackEnd.Share;

public static class ErorrService
{
    public static string GetMessage(ErrorCode? code) => code switch
    {
        ErrorCode.InvalidRefreshToken => "некорректный refresh токен",
        ErrorCode.InvalidAccessToken => "ошибка с access токеном",
        ErrorCode.DeleteTokenError => "Не удалось удалить токен",
        ErrorCode.NotFoundToken => "Не найден рефреш токен",
        ErrorCode.CoockieError => "Не удалось сохранить токен в куки",
        ErrorCode.WrongPassword => "Введен неправильный пароль",
        ErrorCode.RepeatLogin => "Вdеден не уникальный логин",
        ErrorCode.UserCreationError => "Произошла ошибка при создании пользователя",
        ErrorCode.SurnameError => "Фамилия должна быть русскими буквами, причем первая заглавная",
        ErrorCode.SurnameLength => "Фамилия должна быть от 5 до 30 символов",
        ErrorCode.EmptySurname => "Фамилия не должна быть пустой",
        ErrorCode.NameError => "Имя должно быть русскими буквами, причем первая заглавная",
        ErrorCode.NameLength => "Имя должно быть от 2 до 30 символов",
        ErrorCode.EmptyName => "Имя не должно быть пустым",
        ErrorCode.PasswordMatch => "Пароли не совпадают",
        ErrorCode.PasswordError => "Пароль должен быть не менее 8 символов",
        ErrorCode.EmptyPassword => "Пароль не должен быть пустым",
        ErrorCode.EmptyLogin => "Поле логина не должно быть пустым",
        ErrorCode.LoginError => "Почта введена не правильно",
        ErrorCode.UserNotFound => "Пользователь не найден",
        ErrorCode.InvalidPassword => "Логин или пароль неправильный",
        _ => "Неизвестная ошибка"
    };
    public static HttpStatusCode GetStatusCode(ErrorCode? code) => code switch
    {
        ErrorCode.RepeatLogin => HttpStatusCode.Conflict,       // 409
        ErrorCode.UserNotFound => HttpStatusCode.NotFound,       // 404
        ErrorCode.WrongPassword or ErrorCode.InvalidPassword 
        or ErrorCode.SurnameError or ErrorCode.SurnameLength
        or ErrorCode.EmptySurname or ErrorCode.NameError 
        or ErrorCode.NameLength or ErrorCode.EmptyName or
        ErrorCode.PasswordMatch or ErrorCode.PasswordError
        or ErrorCode.EmptyPassword or ErrorCode.EmptyLogin
        or ErrorCode.LoginError or ErrorCode.InvalidAccessToken 
        or ErrorCode.InvalidRefreshToken=> HttpStatusCode.Unauthorized, // 401
        ErrorCode.CoockieError or ErrorCode.UserCreationError
        or ErrorCode.DeleteTokenError or ErrorCode.NotFoundToken => HttpStatusCode.InternalServerError, // 500
        _ => HttpStatusCode.BadRequest                            // 400
    };


    public static IActionResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.isSuccess)
            return new OkObjectResult(result);

        var statusCode = GetStatusCode(result.code);
        var objectResult = new ObjectResult(result)
        {
            StatusCode = (int)statusCode
        };
        return objectResult;
    }
}