using E_Commerce_Application.DTOs.Category;

namespace E_Commerce_Application.Interfaces
{
	public interface ICategoryService
	{
		Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

		Task<CategoryResponseDto?> GetByIdAsync(int id);

		Task<CategoryResponseDto> CreateAsync(CategoryDto dto);

		Task<bool> UpdateAsync(int id, CategoryDto dto);

		Task<bool> DeleteAsync(int id);
	}
}
