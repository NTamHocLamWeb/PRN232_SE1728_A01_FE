using PRN232_SE1728_A01_FE.Services.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.DTOs
{
	public class SystemAccountDTO
	{
		public short AccountId { get; set; }

		public string? AccountName { get; set; }

		[EmailAddress(ErrorMessage = "Please follow email format: abc123@gmail.com")]
		public string? AccountEmail { get; set; }

		public UserRole? AccountRole { get; set; }

		public string? AccountPassword { get; set; }
	}
}
