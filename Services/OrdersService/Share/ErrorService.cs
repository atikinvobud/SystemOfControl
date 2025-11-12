using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace BackEnd.Share;

public static class ErorrService
{
    public static string GetMessage(ErrorCode? code) => code switch
    {
        ErrorCode.ErorUser => "Не правильный пользователь для заказа",
        ErrorCode.productNotFound => "Продукт не найден",
        ErrorCode.StatusNotFound => "Статус не найден",
        ErrorCode.OrderNotFound => "Заказ не найден",
        ErrorCode.InvalidAccessToken => "не валидный access токен",
        _ => "Неизвестная ошибка"
    };
    public static HttpStatusCode GetStatusCode(ErrorCode? code) => code switch
    {
        ErrorCode.ErorUser => HttpStatusCode.Forbidden, //403
        ErrorCode.productNotFound or ErrorCode.StatusNotFound
        or ErrorCode.OrderNotFound => HttpStatusCode.NotFound, // 404
        ErrorCode.InvalidAccessToken => HttpStatusCode.Unauthorized, // 401
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