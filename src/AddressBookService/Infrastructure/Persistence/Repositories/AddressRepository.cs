using AddressBookService.Application.Interfaces;
using AddressBookService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressBookService.Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly AddressBookServiceDbContext _context;

    public AddressRepository(AddressBookServiceDbContext context)
    {
        _context = context;
    }

    public async Task<Address> CreateAddressAsync(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        var addressEntry = await _context.Addresses.AddAsync(address);
        await _context.SaveChangesAsync();
        return addressEntry.Entity;
    }

    public async Task<Address> GetAddressByIdAsync(Guid addressId)
    {
        if (addressId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(addressId));
        }

        return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);
    }

    public async Task<IEnumerable<Address>> GetAllAddressesAsync()
    {
        return await _context.Addresses.ToListAsync();
    }

    public async Task<Address> UpdateAddressAsync(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);

        if (address.Id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(address.Id));
        }

        var updatedAddress = _context.Addresses.Update(address);
        await _context.SaveChangesAsync();

        if (updatedAddress?.Entity is null)
        {
            throw new Exception("Address not found");
        }

        return updatedAddress.Entity;
    }

    public async Task<bool> DeleteAddressAsync(Guid addressId)
    {
        ArgumentNullException.ThrowIfNull(addressId);

        var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);

        if (address is null)
        {
            throw new Exception("Address not found");
        }

        _context.Addresses.Remove(address);
        var deleted = await _context.SaveChangesAsync();

        return deleted > 0;
    }
}