namespace BackEnd.Share;

public static class ErorrService
{
    public static string GetMessage(ErrorCode? code) => code switch
    {
        ErrorCode.UserNotFound => "Пользователь не найден",
        ErrorCode.InvalidPassword => "Логин или пароль неправильный",
        _ => "Неизвестная ошибка"
    };
}