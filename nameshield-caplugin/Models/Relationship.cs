using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class RelationshipData
	{
		[JsonProperty("type")]
		public string type { get; set; }

		[JsonProperty("id")]
		public string id { get; set; }
	}
}
