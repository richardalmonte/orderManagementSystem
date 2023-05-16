using AddressBookService.Api.Contracts.V1.Requests;
using AddressBookService.Api.Contracts.V1.Responses;
using AddressBookService.Api.Controllers.V1;
using AddressBookService.Api.Validators;
using AddressBookService.Application.Interfaces;
using AddressBookService.Domain.Entities;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AddressBookService.Tests.Systems.Controllers.V1;

public class AddressControllerTest
{
    private readonly Fixture _fixture;
    private readonly AddressesController _sut;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IAddressService> _addressService;

    private const string ContextScheme = "http";
    private const string ContextHost = "localhost";
    private const int ContextPort = 5000;

    public AddressControllerTest()
    {
        _fixture = new Fixture();
        _mapper = new Mock<IMapper>();
        _addressService = new Mock<IAddressService>();
        _sut = new AddressesController(_mapper.Object, _addressService.Object);

        _sut.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                Request =
                {
                    Scheme = ContextScheme,
                    Host = new HostString(ContextHost, ContextPort)
                }
            }
        };


        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }


    [Fact]
    public async void CreateAddress_WhenCalledWithValidAddress_ShouldReturn201StatusCode()
    {
        // Arrange
        var addressRequest = _fixture.Create<AddressRegistrationRequest>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<Address>(addressRequest)).Returns(actualAddress);
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.CreateAddressAsync(actualAddress)).ReturnsAsync(actualAddress);

        // Act
        var result = await _sut.CreateAddress(addressRequest);

        // Assert

        result.Should().BeOfType<CreatedResult>();
        var objectResult = result as CreatedResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status201Created);
        objectResult?.Location.Should()
            .Be($"{ContextScheme}://{ContextHost}:{ContextPort}/api/v1/Addresses/{actualAddress.Id}");
    }

    [Fact]
    public async void CreateAddress_WhenCalledWithValidAddress_ShouldCallMapper()
    {
        // Arrange
        var addressRequest = _fixture.Create<AddressRegistrationRequest>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<Address>(addressRequest)).Returns(actualAddress);
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.CreateAddressAsync(actualAddress)).ReturnsAsync(actualAddress);

        // Act
        await _sut.CreateAddress(addressRequest);

        // Assert
        _mapper.Verify(x => x.Map<Address>(addressRequest), Times.Once);
        _mapper.Verify(x => x.Map<AddressResponse>(actualAddress), Times.Once);
    }

    [Fact]
    public async void CreateAddress_WhenCalledWithValidAddress_ShouldCallService()
    {
        // Arrange
        var addressRequest = _fixture.Create<AddressRegistrationRequest>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<Address>(addressRequest)).Returns(actualAddress);
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.CreateAddressAsync(actualAddress)).ReturnsAsync(actualAddress);

        // Act
        await _sut.CreateAddress(addressRequest);

        // Assert
        _addressService.Verify(x => x.CreateAddressAsync(actualAddress), Times.Once);
    }


    [Fact]
    public async void CreateAddress_WhenCalledWithInvalidAddress_ShouldReturn400StatusCode()
    {
        // Arrange
        var addressRequest = _fixture.Build<AddressRegistrationRequest>()
            .Without(u => u.UserId)
            .Create();

        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<Address>(addressRequest)).Returns(actualAddress);
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.CreateAddressAsync(actualAddress)).ReturnsAsync(actualAddress);

        var validator = new AddressRegistrationRequestValidator();
        var result = await validator.ValidateAsync(addressRequest);
        result.AddToModelState(_sut.ModelState, null);

        // Act
        var response = await _sut.CreateAddress(addressRequest);

        // Assert

        response.Should().BeOfType<BadRequestObjectResult>();
        var objectResult = response as BadRequestObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }


    [Fact]
    public async void CreateAddress_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var addressRequest = _fixture.Create<AddressRegistrationRequest>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<Address>(addressRequest)).Returns(actualAddress);
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());

        _addressService.Setup(x => x.CreateAddressAsync(actualAddress)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.CreateAddress(addressRequest);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetAddress_WhenCalledWithValidId_ShouldReturn200StatusCode()
    {
        // Arrange
        var addressId = _fixture.Create<Guid>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync(actualAddress);

        // Act
        var result = await _sut.GetAddress(addressId);

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetAddress_WhenAddressDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null!);

        // Act
        var result = await _sut.GetAddress(addressId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAddress_WhenCalledWithValidId_ShouldCallService()
    {
        // Arrange
        var addressId = _fixture.Create<Guid>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync(actualAddress);

        // Act
        await _sut.GetAddress(addressId);

        // Assert
        _addressService.Verify(x => x.GetAddressByIdAsync(addressId), Times.Once);
    }

    [Fact]
    public async Task GetAddress_WhenCalledWithValidId_ShouldCallMapper()
    {
        // Arrange
        var addressId = _fixture.Create<Guid>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync(actualAddress);

        // Act
        await _sut.GetAddress(addressId);

        // Assert
        _mapper.Verify(x => x.Map<AddressResponse>(actualAddress), Times.Once);
    }

    [Fact]
    public async Task GetAddress_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var addressId = _fixture.Create<Guid>();
        var actualAddress = _fixture.Create<Address>();
        _mapper.Setup(x => x.Map<AddressResponse>(actualAddress))
            .Returns(_fixture.Create<AddressResponse>());
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAddress(addressId);

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async void GetAllAddresses_WhenCalled_ShouldReturn200StatusCode()
    {
        // Arrange
        var actualAddress = _fixture.CreateMany<Address>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<AddressResponse>>(actualAddress))
            .Returns(_fixture.CreateMany<AddressResponse>().ToList());
        _addressService.Setup(x => x.GetAllAddressesAsync()).ReturnsAsync(actualAddress);

        // Act
        var result = await _sut.GetAllAddresses();

        // Assert

        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async void GetAllAddresses_WhenCalled_ShouldCallService()
    {
        // Arrange
        var actualAddress = _fixture.CreateMany<Address>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<AddressResponse>>(actualAddress))
            .Returns(_fixture.CreateMany<AddressResponse>().ToList());
        _addressService.Setup(x => x.GetAllAddressesAsync()).ReturnsAsync(actualAddress);

        // Act
        await _sut.GetAllAddresses();

        // Assert
        _addressService.Verify(x => x.GetAllAddressesAsync(), Times.Once);
    }

    [Fact]
    public async void GetAllAddresses_WhenCalled_ShouldCallMapper()
    {
        // Arrange
        var actualAddresses = _fixture.CreateMany<Address>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<AddressResponse>>(actualAddresses))
            .Returns(_fixture.CreateMany<AddressResponse>().ToList());
        _addressService.Setup(x => x.GetAllAddressesAsync()).ReturnsAsync(actualAddresses);

        // Act
        await _sut.GetAllAddresses();

        // Assert
        _mapper.Verify(x => x.Map<IEnumerable<AddressResponse>>(actualAddresses), Times.Once);
    }

    [Fact]
    public async void GetAllAddresses_WhenServiceThrowsException_ShouldReturn500StatusCode()
    {
        // Arrange
        var actualAddresses = _fixture.CreateMany<Address>().ToList();
        _mapper.Setup(x => x.Map<IEnumerable<AddressResponse>>(actualAddresses))
            .Returns(_fixture.CreateMany<AddressResponse>().ToList());
        _addressService.Setup(x => x.GetAllAddressesAsync()).ThrowsAsync(new Exception());

        // Act
        var result = await _sut.GetAllAddresses();

        // Assert
        result.Should().BeOfType<StatusCodeResult>();
        var objectResult = result as StatusCodeResult;
        objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task UpdateAddress_WhenAddressExists_ReturnsOk()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var address = new Address { Id = addressId };
        var addressUpdateRequest = new AddressUpdateRequest();
        var updatedAddress = new Address();
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync(address);
        _addressService.Setup(x => x.UpdateAddressAsync(It.IsAny<Address>()))
            .ReturnsAsync(updatedAddress);
        _mapper.Setup(x => x.Map(addressUpdateRequest, address)).Returns(updatedAddress);
        var addressResponse = new AddressResponse();
        _mapper.Setup(x => x.Map<AddressResponse>(updatedAddress)).Returns(addressResponse);

        // Act
        var result = await _sut.UpdateAddress(addressId, addressUpdateRequest);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = result as OkObjectResult;
        objectResult?.Value.Should().BeEquivalentTo(addressResponse);
    }

    [Fact]
    public async Task UpdateAddress_WhenAddressDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null!);

        // Act
        var result = await _sut.UpdateAddress(addressId, new AddressUpdateRequest());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteAddress_WhenAddressExists_ReturnsNoContent()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        var address = new Address { Id = addressId };
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync(address);

        // Act
        var result = await _sut.DeleteAddress(addressId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteAddress_WhenAddressDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var addressId = Guid.NewGuid();
        _addressService.Setup(x => x.GetAddressByIdAsync(addressId)).ReturnsAsync((Address)null!);

        // Act
        var result = await _sut.DeleteAddress(addressId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}