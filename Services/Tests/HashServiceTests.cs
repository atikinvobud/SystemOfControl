extern alias Users;
using DTO = Users::BackEnd.DTOs;
using Services =Users::BackEnd.Services;
using Controllers = Users::BackEnd.Controllers;
using Share = Users::BackEnd.Share;

namespace Tests;
public class HashServiceTests
{
    [Fact]
    public async Task Hash_ReturnedHashedString()
    {
        string text = "password12345";

        var hashService = new Services.HashService();
        var hash =  hashService.Hash(text);

        Assert.False(string.IsNullOrEmpty(hash));
        Assert.NotEqual(hash, text);
    }

    [Fact]
    public async Task Verify_ReturnCorrect()
    {
        string text = "password12345";
        var hashService = new Services.HashService();
        var hash =  hashService.Hash(text);

        var result = hashService.Verify(hash,text);
        Assert.True(result);
    }

    [Fact]
    public async Task Verify_ReturnsFalse()
    {
        string text ="password12345";
        var hashService = new Services.HashService();
        var hash =  hashService.Hash(text);

        var result = hashService.Verify(hash,"wrongpassword");
        Assert.False(result);

    }
}