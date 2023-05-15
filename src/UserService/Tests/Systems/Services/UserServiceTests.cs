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
        _context.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);
        _context.Setup(x => x.AddAsync(user, default)).Callback(() => _context.Object.Users.AddAsync(user, default));

        // Act

        var response = await _sut.CreateUserAsync(user);

        // Assert
        _context.Verify(x => x.Users.AddAsync(user, default), Times.Once);
        _context.Verify(x => x.SaveChangesAsync(default), Times.Once);
        response.Should().BeEquivalentTo(user);
    }
}