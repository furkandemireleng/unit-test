namespace unit_test;

using CloudCustomers.API.Model;
using CloudCustomers.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;

using CloudCustomers.API.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task ReturnesStatusCode200()
    {
        //Arrange

        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        var sut = new UserController(mockUserService.Object);

        // Act
        var result = await sut.Get() as NotFoundResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
    }

    [Fact]
    public async Task InvokeUserService()
    {
        //Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        var sut = new UserController(mockUserService.Object);
        //Act
        var result = await sut.Get() as OkResult;
        //assert

        mockUserService.Verify(s => s.GetAllUsers(), Times.Once());
    }

    [Fact]
    public async Task ReturnListOfUser()
    {
        //Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>()
            {
                new ()
                {
                    Id = 1,
                    Name = "furkan",
                    Email = "fsaafs",
                    Address = new Address()
                    {
                        Street = "asd",
                        City = "tavsan",
                        ZipCode = "12345"
                    }
                    
                }
            });

        var sut = new UserController(mockUserService.Object);
        //Act
        var result = await sut.Get();

        result.Should().BeOfType<OkObjectResult>();

        var objectResult = (OkObjectResult)result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }

    [Fact]
    public async Task Return404()
    {
        
        //Arrange
        var mockUserService = new Mock<IUserService>();
        mockUserService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());

        var sut = new UserController(mockUserService.Object);
        //Act
        var result = await sut.Get();

        result.Should().BeOfType<NotFoundResult>();

        var objectResult = (NotFoundResult)result;
        objectResult.StatusCode.Should().Be(404);

    }
}