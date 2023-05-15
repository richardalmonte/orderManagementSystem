using AutoFixture;
using AutoMapper;
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
    private readonly Application.Services.UserService _sut;

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
        
        _userRepository.Verify(x=> x.CreateUserAsync(user), Times.Once);
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
}