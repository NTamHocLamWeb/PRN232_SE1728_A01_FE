using PRN232_SE1728_A01_FE.Services.DTOs;

namespace PRN232_SE1728_A01_FE.Services.Interfaces
{
	public interface ITagService
	{
		Task<List<TagDTO>> GetListTags();
	}
}
