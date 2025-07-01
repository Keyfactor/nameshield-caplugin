using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class ErrorData
	{
		[JsonProperty("errors")]
		public Error Error { get; set; }
	}
	public class Error
	{
		[JsonProperty("code")]
		public string Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
