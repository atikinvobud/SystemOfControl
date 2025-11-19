extern alias Users;
using DTO = Users::BackEnd.DTOs;
using Services =Users::BackEnd.Services;
using Controllers = Users::BackEnd.Controllers;
using Share = Users::BackEnd.Share; 
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Tests;

public class RoleControllerTests
{
    [Fact]
    public async Task GetAllRoles_ReturnsCorrectList()
    {
        var fakeRoles =new List<DTO.GetRole>
        {
            new DTO.GetRole {Id = Guid.NewGuid(), Name ="Admin"},
            new DTO.GetRole {Id = Guid.NewGuid(), Name ="User"}
        };
        var serviceMock =new Mock<Services.IRoleService>();
        serviceMock.Setup(s => s.GetAll()).ReturnsAsync(Share.Result<List<DTO.GetRole>>.Success(fakeRoles));

        var controller = new Controllers.RoleController(serviceMock.Object);

        var result = await controller.GetAllRoles();

       var okResult = Assert.IsType<OkObjectResult>(result);
       var returnedResult = Assert.IsType<Share.Result<List<DTO.GetRole>>>(okResult.Value);
       var listResult =Assert.IsType<List<DTO.GetRole>>(returnedResult.value);
       Assert.Equal(fakeRoles.Count, listResult.Count);
    }

   [Fact]
    public async Task GetAllRoles_ReturnsEmptyList()
    {
        var serviceMock = new Mock<Services.IRoleService>();
        serviceMock.Setup(s => s.GetAll()).ReturnsAsync(Share.Result<List<DTO.GetRole>>.Success(new List<DTO.GetRole>()));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result = await controller.GetAllRoles();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult =Assert.IsType<Share.Result<List<DTO.GetRole>>>(okResult.Value);
        var returnedList =Assert.IsType<List<DTO.GetRole>>(returnedResult.value);
        Assert.Empty(returnedList); 
    }

    [Fact]
    public async Task GetRoleById_ReturnsCorrectRole()
    {
        var serviceMock = new Mock<Services.IRoleService>();
        Guid testId = Guid.NewGuid();
        serviceMock.Setup(s => s.GetById(testId)).ReturnsAsync(Share.Result<DTO.GetRole>.Success(new DTO.GetRole()));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.GetRoleById(testId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<DTO.GetRole>>(okResult.Value);
        var returnedRole =Assert.IsType<DTO.GetRole>(returnedResult.value);
        serviceMock.Verify(s=>s.GetById(testId), Times.Once);
    }

    [Fact]
    public async Task GetRoleById_ReturnsNull()
    {
        var serviceMock = new Mock<Services.IRoleService>();
        Guid testId =Guid.NewGuid();
        serviceMock.Setup(s => s.GetById(testId)).ReturnsAsync(Share.Result<DTO.GetRole>.Error(Share.ErrorCode.RoleNotFound));

        var controller  = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.GetRoleById(testId);

        var objectt =Assert.IsType<ObjectResult>(result);
        Assert.Equal(404, objectt.StatusCode);
        serviceMock.Verify(s => s.GetById(testId), Times.Once);
    }

    [Fact]
    public async Task CreateRole_CreateCorrect()
    {
        DTO.PostRole postRole =new DTO.PostRole()
        {
            Name ="aaaaaa"
        };
        Guid testid =Guid.NewGuid();
        var serviceMock = new Mock<Services.IRoleService>();
        serviceMock.Setup(s => s.CreateRole(postRole)).ReturnsAsync(Share.Result<Guid>.Success(testid));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result = await controller.CreateRole(postRole);

        var objectt = Assert.IsType<OkObjectResult>(result);
        var returnedResult =Assert.IsType<Share.Result<Guid>>(objectt.Value);
        var returnedGuid =Assert.IsType<Guid>(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        Assert.Equal(testid,returnedGuid);
        serviceMock.Verify(s => s.CreateRole(postRole), Times.Once);
    }

    [Fact]
    public async Task DeleteRole_CorrectDelete()
    {
        DTO.DeleteRole deleteRole =new DTO.DeleteRole()
        {
            Id = Guid.NewGuid()
        };
        var serviceMock = new Mock<Services.IRoleService>();
        serviceMock.Setup(s => s.DeleteRole(deleteRole)).ReturnsAsync(Share.Result<bool>.Success(true));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.DeleteRole(deleteRole);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult =Assert.IsType<Share.Result<bool>>(okResult.Value);
        Assert.True(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(s => s.DeleteRole(deleteRole), Times.Once);
    }

    [Fact]
    public async Task DeleteRole_FailDelete()
    {
        var serviceMock = new Mock<Services.IRoleService>();
        DTO.DeleteRole dto =new DTO.DeleteRole();
        serviceMock.Setup(s => s.DeleteRole(dto)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.RoleNotFound));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.DeleteRole(dto);

        var codeObject = Assert.IsType<ObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<bool>>(codeObject.Value);
        Assert.False(returnedResult.isSuccess);

        Assert.Equal(404, codeObject.StatusCode);
        serviceMock.Verify(s =>s.DeleteRole(dto), Times.Once());
    }

    [Fact]
    public async Task UpdateRole_UpdateCorrect()
    {
        DTO.PutRole putRole =new DTO.PutRole()
        {
            Id = Guid.NewGuid(),
            Name ="aaaa"
        };
        var serviceMock = new Mock<Services.IRoleService>();
        serviceMock.Setup(s => s.UpdateRole(putRole)).ReturnsAsync(Share.Result<bool>.Success(true));

        var controller = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.UpdateRole(putRole);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedResult =Assert.IsType<Share.Result<bool>>(okResult.Value);
        Assert.True(returnedResult.value);
        Assert.True(returnedResult.isSuccess);
        serviceMock.Verify(s => s.UpdateRole(putRole), Times.Once);
    }

    [Fact]
    public async Task UpdateRole_UpdateFail()
    {
        var serviceMock = new Mock<Services.IRoleService>();
        DTO.PutRole dto = new DTO.PutRole();
        serviceMock.Setup(s => s.UpdateRole(dto)).ReturnsAsync(Share.Result<bool>.Error(Share.ErrorCode.RoleNotFound));
        
        var controller = new Controllers.RoleController(serviceMock.Object);
        var result =await controller.UpdateRole(dto);

        var codeObject = Assert.IsType<ObjectResult>(result);
        var returnedResult = Assert.IsType<Share.Result<bool>>(codeObject.Value);
        Assert.False(returnedResult.isSuccess);

        Assert.Equal(404, codeObject.StatusCode);
        serviceMock.Verify(s =>s.UpdateRole(dto), Times.Once());
    }
}
