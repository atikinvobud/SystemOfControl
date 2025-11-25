extern alias Users;
using Services =Users.BackEnd.Services;
using Controllers =Users.BackEnd.Controllers;
using Dto = Users.BackEnd.DTOs;
using Share =Users.BackEnd.Share;
using Moq;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Tests;
public class UsersCintrollerTests
{
    [Fact]
    public async Task GetUser_ReturnsCorrectUser()
    {
        Dto.GetUser dto = new Dto.GetUser()
        {
            Id = Guid.NewGuid(),
            Login ="ssss@aaa.ru",
            Name = "Никита",
            SurName ="Дубов"
        };
        Guid testId = Guid.NewGuid();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.GetUser(testId)).ReturnsAsync(Share.Result<Dto.GetUser>.Success(dto));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result =await controller.GetUser(testId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<Dto.GetUser>>(okResult.Value);
        var returnedDto = Assert.IsType<Dto.GetUser>(returnedResult.value);

        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(u => u.GetUser(testId), Times.Once);
    }

    [Fact]
    public async Task GetUser_ReturnsNull()
    {
        Guid testId = Guid.NewGuid();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.GetUser(testId)).ReturnsAsync(Share.Result<Dto.GetUser>.Error(Share.ErrorCode.UserNotFound));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.GetUser(testId);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
        var returnedResult = Assert.IsType<Share.Result<Dto.GetUser>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
    }

    [Fact]
    public async Task GetAll_RetunsList()
    {
        string role ="Admin";
        int page = 2;
        int pageSize = 3;
        List<Dto.GetUser> users = new List<Dto.GetUser>
        {
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="aa" },
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="bb" },   
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="cc" },
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="dd" },
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="ee" },
            new Dto.GetUser() {Id =Guid.NewGuid(),Name ="ff" }
        };
        var pagedResult =Share.Result<List<Dto.GetUser>>.Success(users.Skip(page-1).Take(pageSize).ToList());
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.GetUsersWithPagination(role, page, pageSize)).ReturnsAsync(pagedResult);

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.GetAll(role,page, pageSize);

        var OkObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<List<Dto.GetUser>>>(OkObjectResult.Value);
        var returnedList = Assert.IsType<List<Dto.GetUser>>(returnedResult.value);
        Assert.Equal(3, returnedList.Count);
        serviceMock.Verify(u => u.GetUsersWithPagination(role,page,pageSize), Times.Once);
    }

   [Fact]
    public async Task GetAll_ReturnEmptylist()
    {
        int page = 1;
        int pageSize = 10;
        List<Dto.GetUser> users = new List<Dto.GetUser>();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.GetUsersWithPagination(null, page, pageSize)).ReturnsAsync(Share.Result<List<Dto.GetUser>>.Success(users));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.GetAll(null, page, pageSize);

        var OkObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<List<Dto.GetUser>>>(OkObjectResult.Value);
        var returnedList = Assert.IsType<List<Dto.GetUser>>(returnedResult.value);
        Assert.Empty(returnedList);
        serviceMock.Verify(u => u.GetUsersWithPagination(null,page,pageSize), Times.Once);
    }

    [Fact]
    public async Task UpdateUserInfo_CorrectUpdated()
    {
        var putUserInfo = new Dto.PutUserInfo()
        {
            Id =Guid.NewGuid(),
            Name ="Никита",
            Surname ="Дубов"
        };
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.UpdateUserInfo(putUserInfo)).ReturnsAsync(Share.Result<bool>.Success(true));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.UpdateUserInfo(putUserInfo);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<bool>>(okObjectResult.Value);
        var returnedValue = Assert.IsType<bool>(returnedResult.value);

        Assert.True(returnedValue);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(u => u.UpdateUserInfo(putUserInfo), Times.Once);
    }

    [Fact]
    public async Task UpdateUserInfo_UpdateFail()
    {
        var Dto = new Dto.PutUserInfo();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.UpdateUserInfo(Dto)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.UserInfoNotFound));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.UpdateUserInfo(Dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
        var returnedResult =Assert.IsType<Share.Result<bool>>(objectResult.Value);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(u => u.UpdateUserInfo(Dto), Times.Once);
    }

    [Fact]
    public async Task AppointRole_CorrectCreate()
    {
        var Dto = new Dto.PostUserRole()
        {
          UserId =Guid.NewGuid(),
          RoleId =Guid.NewGuid()  
        };
        Guid testId = Guid.NewGuid();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u =>u.AppointRole(Dto)).ReturnsAsync(Share.Result<Guid>.Success(testId));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.AppointRole(Dto);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<Guid>>(okObjectResult.Value);
        var returnedValue = Assert.IsType<Guid>(returnedResult.value);

        Assert.True(returnedResult.isSuccess);
        Assert.Equal(testId, returnedValue);
        serviceMock.Verify(u => u.AppointRole(Dto), Times.Once);
    }

    [Fact]
    public async Task AppointRole_InvalidUser()
    {
        var Dto =new Dto.PostUserRole();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.AppointRole(Dto)).ReturnsAsync(Share.Result<Guid>.Error(Share.ErrorCode.UserNotFound));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.AppointRole(Dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<Guid>>(objectResult.Value);
        Assert.Equal(404, objectResult.StatusCode);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(u => u.AppointRole(Dto), Times.Once);
    }

    [Fact]
    public async Task AppointRole_InvalidRole()
    {
        var Dto =new Dto.PostUserRole();
        var serviceMock = new Mock<Services.IUserService>();
        serviceMock.Setup(u => u.AppointRole(Dto)).ReturnsAsync(Share.Result<Guid>.Error(Share.ErrorCode.RoleNotFound));

        var controller = new Controllers.UserController(serviceMock.Object);
        var result = await controller.AppointRole(Dto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<Guid>>(objectResult.Value);
        Assert.Equal(404, objectResult.StatusCode);
        Assert.False(returnedResult.isSuccess);
        serviceMock.Verify(u => u.AppointRole(Dto), Times.Once);
    }
}