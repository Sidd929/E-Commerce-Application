using E_Commerce_Application.DTOs.Product;

namespace E_Commerce_Application.Interfaces
{
	public interface IProductService
	{
		Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

		Task<ProductResponseDto?> GetProductByIdAsync(int id);

		Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);

		Task<ProductResponseDto?> UpdateProductAsync(int id,UpdateProductDto dto);

		Task<bool> DeleteProductAsync(int id);
	}
}
