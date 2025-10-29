using Microsoft.VisualBasic;

namespace BackEnd.Share;

public static class ErorrService
{
    public static string GetMessage(ErrorCode? code) => code switch
    {    
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
}