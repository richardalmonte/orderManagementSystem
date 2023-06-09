﻿using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserService.Application.Interfaces;
using UserService.Contracts.V1.Requests;
using UserService.Contracts.V1.Responses;
using UserService.Controllers.V1;
using UserService.Domain.Entities;
using UserService.Validators;

namespace UserService.Tests.Systems.Controllers.V1;

public class UserControllerTest
{
    private readonly Fixture _fixture;
    private readonly UsersController _sut;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IUserService> _userService;

    private const string contextScheme = "http";
    private const string contextHost = "localhost";
    private const int contextPort = 5000;

    public UserControllerTest()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();
        _userService = new Mock<IUserService>();
        _sut = new UsersController(_mapper.Object, _userService.Object);

        _sut.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                Request =
                {
                    Scheme = contextScheme,
                    Host = new HostString(contextHost, contextPort)
                }
            }
        };
    }


    [Fact]
    public async void CreateUser_WhenCalledWithValidUser_ShouldReturn201StatusCode()
    {
        // Arrange
        var userRequest = _fixture.Create<UserRegistrationRequest>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<User>(userRequest)).Returns(actualUser);
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.CreateUserAsync(actualUser)).ReturnsAsync(actualUser);

        // Act
        var result = await _sut.CreateUser(userRequest);

        // Assert

        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult?.Location.Should()
            .Be($"{contextScheme}://{contextHost}:{contextPort}/api/v1/users/{actualUser.Id}");
    }

    [Fact]
    public async void CreateUser_WhenCalledWithValidUser_ShouldCallMapper()
    {
        // Arrange
        var userRequest = _fixture.Create<UserRegistrationRequest>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<User>(userRequest)).Returns(actualUser);
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.CreateUserAsync(actualUser)).ReturnsAsync(actualUser);

        // Act
        await _sut.CreateUser(userRequest);

        // Assert
        _mapper.Verify(x => x.Map<User>(userRequest), Times.Once);
        _mapper.Verify(x => x.Map<UserResponse>(actualUser), Times.Once);
    }

    [Fact]
    public async void CreateUser_WhenCalledWithValidUser_ShouldCallService()
    {
        // Arrange
        var userRequest = _fixture.Create<UserRegistrationRequest>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<User>(userRequest)).Returns(actualUser);
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.CreateUserAsync(actualUser)).ReturnsAsync(actualUser);

        // Act
        await _sut.CreateUser(userRequest);

        // Assert
        _userService.Verify(x => x.CreateUserAsync(actualUser), Times.Once);
    }


    [Fact]
    public async void CreateUser_WhenCalledWithInvalidUser_ShouldReturn400StatusCode()
    {
        // Arrange
        var userRequest = _fixture.Build<UserRegistrationRequest>()
            .Without(u => u.Email)
            .Create();

        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<User>(userRequest)).Returns(actualUser);
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.CreateUserAsync(actualUser)).ReturnsAsync(actualUser);

        var validator = new UserRegistrationRequestValidator();
        var result = validator.Validate(userRequest);
        result.AddToModelState(_sut.ModelState, null);

        // Act
        var response = await _sut.CreateUser(userRequest);

        // Assert

        response.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = response as BadRequestObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [Fact]
    public async void CreateUser_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var userRequest = _fixture.Create<UserRegistrationRequest>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<User>(userRequest)).Returns(actualUser);
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());

        _userService.Setup(x => x.CreateUserAsync(actualUser)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.CreateUser(userRequest);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetUser_WhenCalledWithValidId_ShouldReturn200StatusCode()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(actualUser);

        // Act
        var result = await _sut.GetUser(userId);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetUser_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null!);

        // Act
        var result = await _sut.GetUser(userId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetUser_WhenCalledWithValidId_ShouldCallService()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(actualUser);

        // Act
        await _sut.GetUser(userId);

        // Assert
        _userService.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUser_WhenCalledWithValidId_ShouldCallMapper()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(actualUser);

        // Act
        await _sut.GetUser(userId);

        // Assert
        _mapper.Verify(x => x.Map<UserResponse>(actualUser), Times.Once);
    }

    [Fact]
    public async Task GetUser_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var actualUser = _fixture.Create<User>();
        _mapper.Setup(x => x.Map<UserResponse>(actualUser)).Returns(_fixture.Create<UserResponse>());
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetUser(userId);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
    
    [Fact]
    public async void GetAllUsers_WhenCalled_ShouldReturn200StatusCode()
    {
        // Arrange
        var actualUsers = _fixture.CreateMany<User>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<UserResponse>>(actualUsers)).Returns(_fixture.CreateMany<UserResponse>().ToList());
        _userService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(actualUsers);

        // Act
        var result = await _sut.GetAllUsers();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
    
    [Fact]
    public async void GetAllUsers_WhenCalled_ShouldCallService()
    {
        // Arrange
        var actualUsers = _fixture.CreateMany<User>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<UserResponse>>(actualUsers)).Returns(_fixture.CreateMany<UserResponse>().ToList());
        _userService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(actualUsers);

        // Act
        await _sut.GetAllUsers();

        // Assert
        _userService.Verify(x => x.GetAllUsersAsync(), Times.Once);
    } 
    
    [Fact]
    public async void GetAllUsers_WhenCalled_ShouldCallMapper()
    {
        // Arrange
        var actualUsers = _fixture.CreateMany<User>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<UserResponse>>(actualUsers)).Returns(_fixture.CreateMany<UserResponse>().ToList());
        _userService.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(actualUsers);

        // Act
        await _sut.GetAllUsers();

        // Assert
        _mapper.Verify(x => x.Map<IEnumerable<UserResponse>>(actualUsers), Times.Once);
    }
    
    [Fact]
    public async void GetAllUsers_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var actualUsers = _fixture.CreateMany<User>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<UserResponse>>(actualUsers)).Returns(_fixture.CreateMany<UserResponse>().ToList());
        _userService.Setup(x => x.GetAllUsersAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAllUsers();

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
    
    [Fact]
    public async Task UpdateUser_WhenUserExists_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        var userUpdateRequest = new UserUpdateRequest();
        var updatedUser = new User();
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
        _userService.Setup(x => x.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);
        _mapper.Setup(x => x.Map(userUpdateRequest, user)).Returns(updatedUser);
        var userResponse = new UserResponse();
        _mapper.Setup(x => x.Map<UserResponse>(updatedUser)).Returns(userResponse);

        // Act
        var result = await _sut.UpdateUser(userId, userUpdateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeEquivalentTo(userResponse);
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null!);

        // Act
        var result = await _sut.UpdateUser(userId, new UserUpdateRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ReturnsNoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId };
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _sut.DeleteUser(userId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userService.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null!);

        // Act
        var result = await _sut.DeleteUser(userId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

}