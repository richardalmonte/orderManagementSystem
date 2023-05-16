using AddressBookService.Domain.Entities;

namespace AddressBookService.Application.Interfaces;

public interface IAddressService
{
    Task<Address> CreateAddressAsync(Address address);
    Task<Address> GetAddressByIdAsync(Guid addressId);
    Task<IEnumerable<Address>> GetAllAddressesAsync();
    Task<Address> UpdateAddressAsync(Address address);
    Task DeleteAddressAsync(Guid addressId);
}