using AddressBookService.Domain.Entities;

namespace AddressBookService.Application.Interfaces;

public interface IAddressRepository
{
    Task<Address> CreateAddressAsync(Address address);

    Task<Address> GetAddressByIdAsync(Guid addressId);
    Task<Address> UpdateAddressAsync(Address address);
    Task<bool> DeleteAddressAsync(Guid addressId);
    Task<IEnumerable<Address>> GetAllAddressesAsync();
}