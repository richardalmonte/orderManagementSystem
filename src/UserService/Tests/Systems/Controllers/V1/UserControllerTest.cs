using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.V1.Requests;
using UserService.Controllers.V1;

namespace UserService.Tests.Systems.Controllers.V1;

public class UserControllerTest
{
    private readonly Fixture _fixture;
    private readonly UsersController _sut;

    public UserControllerTest()
    {
        _fixture = new Fixture();
        _sut = new UsersController();
    }


    [Fact]
    public async void CreateUser_WhenCalledWithValidUser_ShouldReturn201StatusCode()
    {
        // Arrange
        var user = _fixture.Create<UserRegistrationRequest>();

        // Act
        var result = (StatusCodeResult)await _sut.CreateUser(user);

        // Assert

        result.Should().BeOfType<StatusCodeResult>();
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
    }
}