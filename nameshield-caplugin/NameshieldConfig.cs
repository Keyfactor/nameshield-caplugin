using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield
{
	public class NameshieldConfig
	{
		public NameshieldConfig() { }

		[JsonProperty("ApiUrl")]
		public string ApiUrl { get; set; }

		[JsonProperty("ApiToken")]
		public string ApiToken { get; set; }

		[JsonProperty("Enabled")]
		public bool Enabled { get; set; } = true;
	}
}
