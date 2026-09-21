using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class ErrorData
	{
		[JsonPropertyName("errors")]
		[JsonProperty("errors")]
		public List<Error> Errors { get; set; }
	}
	public class Error
	{
		[JsonPropertyName("status")]
		[JsonProperty("status")]
		public string Status { get; set; }
		[JsonPropertyName("title")]
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonPropertyName("detail")]
		[JsonProperty("detail")]
		public string Detail { get; set; }
	}
}
