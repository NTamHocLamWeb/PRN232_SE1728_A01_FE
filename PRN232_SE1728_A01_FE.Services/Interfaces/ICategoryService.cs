using PRN232_SE1728_A01_FE.Services.DTOs;

namespace PRN232_SE1728_A01_FE.Services.Interfaces
{
	public interface ICategoryService
	{
		Task<List<CategoryDTO>> GetAllListAsync();
		Task<CategoryDTO> GetCategoryByid(short id);
		Task<bool> UpdateCategory(CategoryDTO category);
		Task<bool> CreateCategory(CategoryDTO category);
		Task<bool> DeleteCategory(short id);
		Task<List<CategoryDTO>> SearchByName(string name);
	}
}
