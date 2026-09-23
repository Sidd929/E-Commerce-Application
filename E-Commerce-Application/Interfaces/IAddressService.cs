using E_Commerce_Application.DTOs.Address;
using E_Commerce_Application.Entities;

namespace E_Commerce_Application.Interfaces
{
	public interface IAddressService
	{
		Task<IEnumerable<AddressResponseDto>> GetByUserIdAsync(int userId);
		Task<AddressResponseDto?> GetByIdAsync(int id,int userId);
		Task<AddressResponseDto> CreateAsync(int userId,AddressDto address);
		Task<bool> UpdateAsync(int id,int userId,AddressDto address);
		Task<bool> DeleteAsync(int id, int userId);
	}
}
