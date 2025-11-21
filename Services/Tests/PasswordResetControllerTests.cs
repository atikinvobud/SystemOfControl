extern alias Users;
using DTO = Users::BackEnd.DTOs;
using Services =Users::BackEnd.Services;
using Controllers = Users::BackEnd.Controllers;
using Share = Users::BackEnd.Share;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Tests;
public class PasswordResetControllerTests
{
    [Fact]
    public async Task RequsetRecovery_ReturnCode()
    {
        DTO.RequestRecoveryDto dto = new DTO.RequestRecoveryDto(){Email ="aaa@aa.ru"};
        string code = "111111";
        var serviceMock = new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.GenerateRecoveryCodeAsync(dto)).ReturnsAsync(Share.Result<string>.Success(code));

        var controller = new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.RequestRecovery(dto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<string>>(okObject.Value);
        var returndeString = Assert.IsType<string>(returnedResult.value);
        Assert.Equal(code, returndeString);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(s => s.GenerateRecoveryCodeAsync(dto), Times.Once);
    }
    
    [Fact]
    public async Task RequestRecovery_ReturnedEmptyName()
    {
        DTO.RequestRecoveryDto dto = new DTO.RequestRecoveryDto();
        var serviceMock = new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.GenerateRecoveryCodeAsync(dto)).ReturnsAsync(Share.Result<string>.Error(Share.ErrorCode.EmptyName));

        var controller = new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.RequestRecovery(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult =Assert.IsType<Share.Result<string>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.GenerateRecoveryCodeAsync(dto), Times.Once);
    }
    [Fact]
    public async Task RequestRecovery_ReturnedRecoveryError()
    {
        DTO.RequestRecoveryDto dto = new DTO.RequestRecoveryDto();
        var serviceMock = new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.GenerateRecoveryCodeAsync(dto)).ReturnsAsync(Share.Result<string>.Error(Share.ErrorCode.RecoveryCodeError));

        var controller = new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.RequestRecovery(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<string>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.GenerateRecoveryCodeAsync(dto), Times.Once);
    }

    [Fact]
    public async Task VerifyCode_ReturnCorrect()
    {
        DTO.VerifyCodeDto dto = new DTO.VerifyCodeDto();
        string? token = Guid.NewGuid().ToString();
        var serviceMock =new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.VerifyCodeAndGenerateTokenAsync(dto)).ReturnsAsync(Share.Result<string?>.Success(token));

        var controller =new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.VerifyCode(dto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<string?>>(okObject.Value);
        var returnedString = Assert.IsType<string?>(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        Assert.Equal(token, returnedString);
        serviceMock.Verify(s => s.VerifyCodeAndGenerateTokenAsync(dto), Times.Once);
    }

    [Fact]
    public async Task VerifyCode_ReturnEmptyName()
    {
        DTO.VerifyCodeDto dto = new DTO.VerifyCodeDto();
        var serviceMock =new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.VerifyCodeAndGenerateTokenAsync(dto)).ReturnsAsync(Share.Result<string?>.Error(Share.ErrorCode.EmptyName));
    
        var controller = new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.VerifyCode(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<string?>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.VerifyCodeAndGenerateTokenAsync(dto), Times.Once);
    }

   [Fact]
    public async Task VerifyCode_ReturnRecoveryCodeError()
    {
        DTO.VerifyCodeDto dto = new DTO.VerifyCodeDto();
        var serviceMock = new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.VerifyCodeAndGenerateTokenAsync(dto)).ReturnsAsync(Share.Result<string?>.Error(Share.ErrorCode.RecoveryCodeError));

        var controller = new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.VerifyCode(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<string?>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.VerifyCodeAndGenerateTokenAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_ReturnCorrect()
    {
        DTO.ResetPasswordDto dto = new DTO.ResetPasswordDto();
        var serviceMock =new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.ResetPasswordAsync(dto)).ReturnsAsync(Share.Result<bool>.Success(true));

        var controller =new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.ResetPassword(dto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<bool>>(okObject.Value);
        var returnedBool = Assert.IsType<bool>(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        Assert.True(returnedBool);
        serviceMock.Verify(s => s.ResetPasswordAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_ReturnEmptyName()
    {
        DTO.ResetPasswordDto dto = new DTO.ResetPasswordDto();
        var serviceMock =new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.ResetPasswordAsync(dto)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.EmptyName));

        var controller =new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.ResetPassword(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<bool>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.ResetPasswordAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_ReturnRecoveryCodeError()
    {
        DTO.ResetPasswordDto dto = new DTO.ResetPasswordDto();
        var serviceMock =new Mock<Services.IPasswordRecoveryService>();
        serviceMock.Setup(s => s.ResetPasswordAsync(dto)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.RecoveryCodeError));

        var controller =new Controllers.PasswordRecoveryController(serviceMock.Object);
        var result = await controller.ResetPassword(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<bool>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.ResetPasswordAsync(dto), Times.Once);
    }
}