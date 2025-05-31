using PRN232_SE1728_A01_FE.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.Interfaces
{
	public interface INewsArticleService
	{
		Task<List<NewsArticleDTO>> GetNewsArticlesList();
		Task<List<NewsArticleDTO>> GetNewsArticlesHistory(short id);
		Task<bool> CreateNewsArticle(NewsArticleDTO newNewsArticleDTO);
		Task<NewsArticleDTO> GetNewsById(string id);
		Task<bool> DeleteNews(string id);
		Task<List<NewsArticleDTO>> SearchByTittle(string search);
		Task<bool> UpdateNews(NewsArticleDTO editNews);
		Task<List<NewsArticleDTO>> FilterByDate(DateTime fromDate, DateTime toDate);
	}
}
