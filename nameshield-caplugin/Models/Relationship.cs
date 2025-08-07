using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class RelationshipData
	{
		[JsonPropertyName("type")]
		[JsonProperty("type")]
		public string type { get; set; }

		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string id { get; set; }
	}
}
