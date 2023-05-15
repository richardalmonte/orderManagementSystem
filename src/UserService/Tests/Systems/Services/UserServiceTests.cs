using AutoFixture;
using FluentAssertions;
using Moq;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Infrastructure.Persistence;


namespace UserService.Tests.Systems.Services;

public class UserServiceTests
{
    private readonly Mock<UserServiceDbContext> _context;

    private readonly Mock<IUserRepository> _userRepository;
    private readonly Fixture _fixture;
    private readonly IUserService _sut;

    public UserServiceTests()
    {
        _context = new Mock<UserServiceDbContext>();
        _fixture = new Fixture();
        _userRepository = new Mock<IUserRepository>();
        _sut = new Application.Services.UserService(_userRepository.Object);
    }


    [Fact]
    public async void CreateUserAsync_ShouldCreateUser_WhenCalledWithValidUser()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _userRepository.Setup(x => x.CreateUserAsync(user)).ReturnsAsync(user);

        // Act

        var response = await _sut.CreateUserAsync(user);

        // Assert

        _userRepository.Verify(x => x.CreateUserAsync(user), Times.Once);
        response.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async void CreateUserAsync_ShouldThrowArgumentNullException_WhenCalledWithNullUser()
    {
        // Arrange
        User user = null;

        // Act
        var action = async () => await _sut.CreateUserAsync(user);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetUserByIdAsync_ShouldReturnUser_WhenCalledWithValidId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _userRepository.Setup(x => x.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

        // Act
        var response = await _sut.GetUserByIdAsync(user.Id);

        // Assert
        _userRepository.Verify(x => x.GetUserByIdAsync(user.Id), Times.Once);
        response.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenCalledWithInvalidId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

        // Act
        var result = await _sut.GetUserByIdAsync(userId);

        // Assert
        _userRepository.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public async void GetUserByIdAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var userId = Guid.Empty;

        // Act
        var action = async () => await _sut.GetUserByIdAsync(userId);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetAllUsersAsync_ShouldReturnAllUsers_WhenCalled()
    {
        // Arrange
        var users = _fixture.CreateMany<User>().ToList();
        _userRepository.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var response = await _sut.GetAllUsersAsync();

        // Assert
        _userRepository.Verify(x => x.GetAllUsersAsync(), Times.Once);
        response.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async void GetAllUsersAsync_ShouldReturnEmptyList_WhenCalled()
    {
        // Arrange
        var users = new List<User>();
        _userRepository.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

        // Act
        var response = await _sut.GetAllUsersAsync();

        // Assert
        _userRepository.Verify(x => x.GetAllUsersAsync(), Times.Once);
        response.Should().BeEquivalentTo(users);
    }


    [Fact]
    public async void UpdateUserAsync_ShouldUpdateUser_WhenCalledWithValidUser()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _userRepository.Setup(x => x.UpdateUserAsync(user)).ReturnsAsync(user);

        // Act
        var response = await _sut.UpdateUserAsync(user);

        // Assert
        _userRepository.Verify(x => x.UpdateUserAsync(user), Times.Once);
        response.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldThrowArgumentNullException_WhenCalledWithNullUser()
    {
        // Arrange
        User user = null;

        // Act
        var action = async () => await _sut.UpdateUserAsync(user);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateUserAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var user = _fixture.Create<User>();
        user.Id = Guid.Empty;

        // Act
        var action = async () => await _sut.UpdateUserAsync(user);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldDeleteUser_WhenCalledWithValidId()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepository.Setup(x => x.DeleteUserAsync(userId)).ReturnsAsync(true);

        // Act
        await _sut.DeleteUserAsync(userId);

        // Assert
        _userRepository.Verify(x => x.DeleteUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var userId = Guid.Empty;

        // Act
        var action = new Func<Task>(async () => await _sut.DeleteUserAsync(userId));

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}