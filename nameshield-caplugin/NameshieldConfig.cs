using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield
{
	public class NameshieldConfig
	{
		public NameshieldConfig() { }

		[JsonPropertyName("ApiUrl")]
		[JsonProperty("ApiUrl")]
		public string ApiUrl { get; set; }

		[JsonPropertyName("ApiToken")]
		[JsonProperty("ApiToken")]
		public string ApiToken { get; set; }

		[JsonPropertyName("Enabled")]
		[JsonProperty("Enabled")]
		public bool Enabled { get; set; } = true;
	}
}
