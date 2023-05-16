using AddressBookService.Application.Interfaces;
using AddressBookService.Application.Services;
using AddressBookService.Domain.Entities;
using AddressBookService.Infrastructure.Persistence;
using AutoFixture;
using FluentAssertions;
using Moq;

namespace AddressBookService.Tests.Systems.Services;

public class AddressServiceTests
{
    private readonly Mock<AddressBookServiceDbContext> _context;

    private readonly Mock<IAddressRepository> _addressRepository;
    private readonly Fixture _fixture;
    private readonly IAddressService _sut;

    public AddressServiceTests()
    {
        _context = new Mock<AddressBookServiceDbContext>();
        _fixture = new Fixture();
        _addressRepository = new Mock<IAddressRepository>();
        _sut = new AddressService(_addressRepository.Object);

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateAddressAsync_ShouldCreateAddress_WhenCalledWithValidAddress()
    {
        // Arrange
        var address = _fixture.Create<Address>();
        _addressRepository.Setup(x => x.CreateAddressAsync(address)).ReturnsAsync(address);

        // Act

        var response = await _sut.CreateAddressAsync(address);

        // Assert

        _addressRepository.Verify(x => x.CreateAddressAsync(address), Times.Once);
        response.Should().BeEquivalentTo(address);
    }

    [Fact]
    public async void CreateAddressAsync_ShouldThrowArgumentNullException_WhenCalledWithNullAddress()
    {
        // Arrange
        Address address = null!;

        // Act
        var action = async () => await _sut.CreateAddressAsync(address);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetAddressByIdAsync_ShouldReturnAddress_WhenCalledWithValidId()
    {
        // Arrange
        var address = _fixture.Create<Address>();
        _addressRepository.Setup(x => x.GetAddressByIdAsync(address.Id)).ReturnsAsync(address);

        // Act
        var response = await _sut.GetAddressByIdAsync(address.Id);

        // Assert
        _addressRepository.Verify(x => x.GetAddressByIdAsync(address.Id), Times.Once);
        response.Should().BeEquivalentTo(address);
    }

    [Fact]
    public async Task GetAddressByIdAsync_ShouldReturnNull_WhenCalledWithInvalidId()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        _addressRepository.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null!);

        // Act
        var result = await _sut.GetAddressByIdAsync(addressId);

        // Assert
        _addressRepository.Verify(x => x.GetAddressByIdAsync(addressId), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public async void GetAddressByIdAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var addressId = Guid.Empty;

        // Act
        var action = async () => await _sut.GetAddressByIdAsync(addressId);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void GetAllAddressesAsync_ShouldReturnAllAddresses_WhenCalled()
    {
        // Arrange
        var addresses = _fixture.CreateMany<Address>().ToList();
        _addressRepository.Setup(x => x.GetAllAddressesAsync()).ReturnsAsync(addresses);

        // Act
        var response = await _sut.GetAllAddressesAsync();

        // Assert
        _addressRepository.Verify(x => x.GetAllAddressesAsync(), Times.Once);
        response.Should().BeEquivalentTo(addresses);
    }

    [Fact]
    public async void GetAllAddressesAsync_ShouldReturnEmptyList_WhenCalled()
    {
        // Arrange
        var addresses = new List<Address>();
        _addressRepository.Setup(x => x.GetAllAddressesAsync()).ReturnsAsync(addresses);

        // Act
        var response = await _sut.GetAllAddressesAsync();

        // Assert
        _addressRepository.Verify(x => x.GetAllAddressesAsync(), Times.Once);
        response.Should().BeEquivalentTo(addresses);
    }


    [Fact]
    public async void UpdateAddressAsync_ShouldUpdateAddress_WhenCalledWithValidAddress()
    {
        // Arrange
        var address = _fixture.Create<Address>();
        _addressRepository.Setup(x => x.UpdateAddressAsync(address)).ReturnsAsync(address);

        // Act
        var response = await _sut.UpdateAddressAsync(address);

        // Assert
        _addressRepository.Verify(x => x.UpdateAddressAsync(address), Times.Once);
        response.Should().BeEquivalentTo(address);
    }

    [Fact]
    public async Task UpdateAddressAsync_ShouldThrowArgumentNullException_WhenCalledWithNullAddress()
    {
        // Arrange
        Address address = null!;

        // Act
        var action = async () => await _sut.UpdateAddressAsync(address);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async void UpdateAddressAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var address = _fixture.Create<Address>();
        address.Id = Guid.Empty;

        // Act
        var action = async () => await _sut.UpdateAddressAsync(address);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteAddressAsync_ShouldDeleteAddress_WhenCalledWithValidId()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        _addressRepository.Setup(x => x.DeleteAddressAsync(addressId)).ReturnsAsync(true);

        // Act
        await _sut.DeleteAddressAsync(addressId);

        // Assert
        _addressRepository.Verify(x => x.DeleteAddressAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task DeleteAddressAsync_ShouldThrowArgumentNullException_WhenCalledWithEmptyId()
    {
        // Arrange
        var addressId = Guid.Empty;

        // Act
        var action = new Func<Task>(async () => await _sut.DeleteAddressAsync(addressId));

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }
}