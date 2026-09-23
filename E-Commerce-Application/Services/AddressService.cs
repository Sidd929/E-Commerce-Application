using E_Commerce_Application.Data;
using E_Commerce_Application.DTOs.Address;
using E_Commerce_Application.Entities;
using E_Commerce_Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Application.Services
{
	public class AddressService : IAddressService
	{
		private readonly AppDbContext _context;

		public AddressService(AppDbContext context)
		{
			_context = context;
		}

		public async Task<AddressResponseDto> CreateAsync(int userId, AddressDto address)
		{
			if (address.IsDefault) { 
				var existingDefaultAddress = await _context.Addresses.Where(a => a.UserId == userId && a.IsDefault == true).ToListAsync();

				foreach (var existingAddress in existingDefaultAddress) { 
					existingAddress.IsDefault = false;
				}
			}

			var newAddress = new Address()
			{
				UserId = userId,
				AddressLine1 = address.AddressLine1,
				AddressLine2 = address.AddressLine2,
				City = address.City,
				State = address.State,
				PostalCode = address.PostalCode,
				Country = address.Country,
				IsDefault = address.IsDefault
			};

			_context.Addresses.Add(newAddress);
			await _context.SaveChangesAsync();

			return new AddressResponseDto
			{
				Id = newAddress.Id,
				AddressLine1 = newAddress.AddressLine1,
				AddressLine2 = newAddress.AddressLine2,
				City = newAddress.City,
				State = newAddress.State,
				PostalCode = newAddress.PostalCode,
				Country = newAddress.Country,
				IsDefault = newAddress.IsDefault
			};
		}

		public async Task<bool> DeleteAsync(int id, int userId)
		{
			var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

			if (address == null) 
			{
				return false;
			}
			_context.Addresses.Remove(address);

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<AddressResponseDto?> GetByIdAsync(int id, int userId)
		{
			return await _context.Addresses
			   .Where(a => a.Id == id && a.UserId == userId)
			   .Select(a => new AddressResponseDto
			   {
				   Id = a.Id,
				   AddressLine1 = a.AddressLine1,
				   AddressLine2 = a.AddressLine2,
				   City = a.City,
				   State = a.State,
				   PostalCode = a.PostalCode,
				   Country = a.Country,
				   IsDefault = a.IsDefault
			   })
			   .FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<AddressResponseDto>> GetByUserIdAsync(int userId)
		{
			return await _context.Addresses
			   .Where(a => a.UserId == userId)
			   .Select(a => new AddressResponseDto
			   {
				   Id = a.Id,
				   AddressLine1 = a.AddressLine1,
				   AddressLine2 = a.AddressLine2,
				   City = a.City,
				   State = a.State,
				   PostalCode = a.PostalCode,
				   Country = a.Country,
				   IsDefault = a.IsDefault
			   })
			   .ToListAsync();
		}

		public async Task<bool> UpdateAsync(int id, int userId, AddressDto address)
		{
			var existingAddress = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

			if (existingAddress == null)
				return false;

			if (address.IsDefault)
			{
				var existingDefaultAddresses = await _context.Addresses
					.Where(a =>
						a.UserId == userId &&
						a.IsDefault &&
						a.Id != id)
					.ToListAsync();

				foreach (var defaultaddress in existingDefaultAddresses)
				{
					defaultaddress.IsDefault = false;
				}
			}

			existingAddress.AddressLine1 = address.AddressLine1;
			existingAddress.AddressLine2 = address.AddressLine2;
			existingAddress.City = address.City;
			existingAddress.State = address.State;
			existingAddress.PostalCode = address.PostalCode;
			existingAddress.Country = address.Country;
			existingAddress.IsDefault = address.IsDefault;

			await _context.SaveChangesAsync();

			return true;
		}
	}
}
