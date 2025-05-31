using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRN232_SE1728_A01_FE.Services.DTOs
{
	public class ODataResponse<T>
	{
		[JsonPropertyName("value")]
		public List<T> Value { get; set; }
	}
}
