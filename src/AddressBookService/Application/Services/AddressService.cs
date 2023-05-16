using AddressBookService.Application.Interfaces;
using AddressBookService.Domain.Entities;

namespace AddressBookService.Application.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public Task<Address> CreateAddressAsync(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        return _addressRepository.CreateAddressAsync(address);
    }

    public async Task<Address> GetAddressByIdAsync(Guid addressId)
    {
        ArgumentNullException.ThrowIfNull(addressId);

        if (addressId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(addressId));
        }

        return await _addressRepository.GetAddressByIdAsync(addressId);
    }

    public Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        return _addressRepository.GetAllAddressesAsync();
    }

    public async Task<Address> UpdateAddressAsync(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(address.Id));
        }

        return await _addressRepository.UpdateAddressAsync(address);
    }

    public Task DeleteAddressAsync(Guid addressId)
    {
        ArgumentNullException.ThrowIfNull(addressId);

        if (addressId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(addressId));
        }

        return _addressRepository.DeleteAddressAsync(addressId);
    }
}