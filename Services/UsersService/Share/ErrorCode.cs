namespace BackEnd.Share;

public enum ErrorCode
{
    InvalidAccessToken,
    InvalidRefreshToken,
    NotFoundToken,
    DeleteTokenError,
    CoockieError,
    WrongPassword,
    EmptyLogin,
    RepeatLogin,
    SurnameError,
    EmptySurname,
    SurnameLength,
    NameError,
    EmptyName,
    NameLength,
    EmptyPassword,
    LoginError,
    PasswordError,
    PasswordMatch,
    UserCreationError,
    UserNotFound,
    InvalidPassword
}