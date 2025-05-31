using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.DTOs
{
	public class CategoryDTO
	{
		public short CategoryId { get; set; }

		public string CategoryName { get; set; } = null!;

		public string CategoryDesciption { get; set; } = null!;

		public string ParentCategory { get; set; } = null!;

		public short? ParentCategoryId { get; set; } = null!;

		public bool? IsActive { get; set; }
	}
}
