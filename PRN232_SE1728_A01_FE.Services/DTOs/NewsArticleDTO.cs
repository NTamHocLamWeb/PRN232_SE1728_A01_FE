using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.DTOs
{
	public class NewsArticleDTO
	{
		public string NewsArticleId { get; set; } = null!;

		public string? NewsTitle { get; set; }

		public string Headline { get; set; } = null!;

		public DateTime? CreatedDate { get; set; }

		public string? NewsContent { get; set; }

		public string? NewsSource { get; set; }

		public short? CategoryId { get; set; }

		public bool? NewsStatus { get; set; }

		public short? CreatedById { get; set; }

		public short? UpdatedById { get; set; }

		public DateTime? ModifiedDate { get; set; }

		public string CategoryName { get; set; }

		public string CreatedBy { get; set; }

		public string UpdatedBy { get; set; }

		public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
	}
}
