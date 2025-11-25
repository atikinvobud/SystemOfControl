extern alias Users;
using DTO = Users::BackEnd.DTOs;
using Services =Users::BackEnd.Services;
using Controllers = Users::BackEnd.Controllers;
using Share = Users::BackEnd.Share;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Tests;
public class CoockieServiceTests
{
    [Theory]
    [InlineData("token1", "abc", 1)]
    [InlineData("token2", "123", 19)]
    [InlineData("token3", "xyz", 30)]
    public async Task SetCookie_ReturnsCorrect(string name, string token, int days)
    {
        var context =new DefaultHttpContext();
        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(s => s.HttpContext).Returns(context);

        var service = new Services.CoockieService(accessorMock.Object);
        var result = service.SetCookie(name,token,days);

        Assert.True(result);
        var setCookieHeader = context.Response.Headers["Set-Cookie"].ToString();
        Assert.Contains(name, setCookieHeader);
        Assert.Contains(token, setCookieHeader);
    }

    [Theory]
    [InlineData("token1", "abc", 1)]
    public async Task SetCookie_ReturnsNull(string name, string token, int days)
    {
        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(s => s.HttpContext).Returns((HttpContext?)null);

        var service = new Services.CoockieService(accessorMock.Object);
        var result = service.SetCookie(name,token,days);

        Assert.False(result);
    }

    [Theory]
    [InlineData("aaaa")]
    public async Task GetCookie_ReturnsNull(string key)
    {
        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(s => s.HttpContext).Returns((HttpContext?)null);

        var service = new Services.CoockieService(accessorMock.Object);
        var result = service.GetCookie(key);

        Assert.Null(result);
    }
    
    [Theory]
    [InlineData("token")]
    public async Task GetCookie_ReturnsCorrect(string key)
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Cookie"]=$"{key}=12345";

        var accessorMock = new Mock<IHttpContextAccessor>();
        accessorMock.Setup(a => a.HttpContext).Returns(context);

        var service = new Services.CoockieService(accessorMock.Object);
        var result = service.GetCookie($"{key}");

        Assert.NotNull(result);
        Assert.Equal("12345", result);
    }

    [Fact]
public void DeleteCookie_RemovesCookie()
{

    var context = new DefaultHttpContext();
    var accessorMock = new Mock<IHttpContextAccessor>();
    accessorMock.Setup(a => a.HttpContext).Returns(context);

    var service = new Services.CoockieService(accessorMock.Object);
    service.DeleteCookie("token");
    Assert.Contains("token=", context.Response.Headers["Set-Cookie"].ToString());
    Assert.Contains("expires=", context.Response.Headers["Set-Cookie"].ToString());
}

}