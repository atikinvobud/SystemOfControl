extern alias Users;
using DTO = Users::BackEnd.DTOs;
using Services =Users::BackEnd.Services;
using Controllers = Users::BackEnd.Controllers;
using Share = Users::BackEnd.Share;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace Tests;
public class AuthorizationControllerTests
{
    [Fact]
    public async Task Registr_ReturnCorrect()
    {
        DTO.RegistrDTO dto = new DTO.RegistrDTO();
        Guid? testId = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.RegistrUser(dto)).ReturnsAsync(Share.Result<Guid?>.Success(testId));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Registr(dto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<Guid?>>(okObject.Value);
        var returnedGuid = Assert.IsType<Guid>(returnedResult.value);
        Assert.Equal(testId, returnedGuid);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RegistrUser(dto), Times.Once);
    }

    [Fact]
    public async Task Reqistr_ReturnRepeatLogin()
    {
        DTO.RegistrDTO dto = new DTO.RegistrDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.RegistrUser(dto)).ReturnsAsync(Share.Result<Guid?>.Error(Share.ErrorCode.RepeatLogin));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Registr(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(409, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<Guid?>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RegistrUser(dto), Times.Once);
    }

    [Fact]
    public async Task Reqistr_ReturnUserCreationError()
    {
        DTO.RegistrDTO dto = new DTO.RegistrDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.RegistrUser(dto)).ReturnsAsync(Share.Result<Guid?>.Error(Share.ErrorCode.UserCreationError));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Registr(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<Guid?>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RegistrUser(dto), Times.Once);
    }

    [Fact]
    public async Task Regist_ReturnEptyName()
    {
        DTO.RegistrDTO dto = new DTO.RegistrDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.RegistrUser(dto)).ReturnsAsync(Share.Result<Guid?>.Error(Share.ErrorCode.EmptyName));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Registr(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<Guid?>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RegistrUser(dto), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnCorrect()
    {
        DTO.LoginDTO dto = new DTO.LoginDTO();
        DTO.AnswerLoginDTO answer =new DTO.AnswerLoginDTO(); 
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.Login(dto)).ReturnsAsync(Share.Result<DTO.AnswerLoginDTO>.Success(answer));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Login(dto);

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<DTO.AnswerLoginDTO>>(okObject.Value);
        var returnedDTO = Assert.IsType<DTO.AnswerLoginDTO>(returnedResult.value);
        Assert.Equal(answer, returnedDTO);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Login(dto), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnUserNotFound()
    {
        DTO.LoginDTO dto = new DTO.LoginDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.Login(dto)).ReturnsAsync(Share.Result<DTO.AnswerLoginDTO>.Error(Share.ErrorCode.UserNotFound));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Login(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.AnswerLoginDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Login(dto), Times.Once);
    }
    [Fact]
    public async Task Login_ReturnWrongPassword()
    {
        DTO.LoginDTO dto = new DTO.LoginDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.Login(dto)).ReturnsAsync(Share.Result<DTO.AnswerLoginDTO>.Error(Share.ErrorCode.WrongPassword));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Login(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.AnswerLoginDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Login(dto), Times.Once);
    }

    [Fact]
    public async Task Login_ReturnCookieError()
    {
        DTO.LoginDTO dto = new DTO.LoginDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        serviceMock.Setup(s => s.Login(dto)).ReturnsAsync(Share.Result<DTO.AnswerLoginDTO>.Error(Share.ErrorCode.CoockieError));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Login(dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.AnswerLoginDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Login(dto), Times.Once);
    }

    [Fact]
    public async Task Logout_ReturnsUnAthorized()
    {
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns((Guid?)null);

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Logout();

        var badResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, badResult.StatusCode); 
        serviceMock.Verify(s => s.Logout(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Logout_ReturnsCorrect()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.Logout(Id)).ReturnsAsync(Share.Result<bool>.Success(true));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Logout();

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<bool>>(okObject.Value);
        var returnedDTO = Assert.IsType<bool>(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        Assert.True(returnedDTO);
        serviceMock.Verify(s => s.Logout(Id), Times.Once);
    }

    [Fact]
    public async Task Logout_ReturnNotFoundToken()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.Logout(Id)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.NotFoundToken));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Logout();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<bool>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Logout(Id), Times.Once);
    }

    [Fact]
    public async Task Logout_ReturnDeleteTokenError()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.Logout(Id)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.DeleteTokenError));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Logout();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<bool>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.Logout(Id), Times.Once);
    }

    [Fact]
    public async Task Refresh_ReturnsUnAthorized()
    {
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns((Guid?)null);

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var badResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, badResult.StatusCode); 
        serviceMock.Verify(s => s.Logout(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Refresh_ReturnsCorrect()
    {
        var Id = Guid.NewGuid();
        var dto = new DTO.ChangeTokenDTO();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.RefreshToken(Id)).ReturnsAsync(Share.Result<DTO.ChangeTokenDTO>.Success(dto));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var okObject = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<DTO.ChangeTokenDTO>>(okObject.Value);
        var returnedDTO = Assert.IsType<DTO.ChangeTokenDTO>(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        Assert.Equal(dto, returnedDTO);
        serviceMock.Verify(s => s.RefreshToken(Id), Times.Once);
    }

    [Fact]
    public async Task Refresh_ReturnNotFoundToken()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.RefreshToken(Id)).ReturnsAsync(Share.Result<DTO.ChangeTokenDTO>.Error(Share.ErrorCode.NotFoundToken));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.ChangeTokenDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RefreshToken(Id), Times.Once);
    }
    [Fact]
    public async Task Refresh_ReturnInvalidRefreshToken()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.RefreshToken(Id)).ReturnsAsync(Share.Result<DTO.ChangeTokenDTO>.Error(Share.ErrorCode.InvalidRefreshToken));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(401, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.ChangeTokenDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RefreshToken(Id), Times.Once);
    }

    [Fact]
    public async Task Refresh_ReturnUserNotFound()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.RefreshToken(Id)).ReturnsAsync(Share.Result<DTO.ChangeTokenDTO>.Error(Share.ErrorCode.UserNotFound));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.ChangeTokenDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RefreshToken(Id), Times.Once);
    }

    [Fact]
    public async Task Refresh_ReturnCoockieError()
    {
        var Id = Guid.NewGuid();
        var serviceMock = new Mock<Services.IAuthorizationServic>();
        var accessorMock = new Mock<Services.ITokenAccessor>();
        accessorMock.Setup(s => s.GetUserId()).Returns(Id);
        serviceMock.Setup(s => s.RefreshToken(Id)).ReturnsAsync(Share.Result<DTO.ChangeTokenDTO>.Error(Share.ErrorCode.CoockieError));

        var controller = new Controllers.AuthorizationController(serviceMock.Object, accessorMock.Object);
        var result = await controller.Refresh();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<DTO.ChangeTokenDTO>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(s => s.RefreshToken(Id), Times.Once);
    }
}