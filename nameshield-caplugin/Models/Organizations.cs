using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class OrganizationData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public List<Organization> Organizations { get; set; }
	}

	public class Organization
	{
		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public OrganizationAttributes Attributes { get; set; }
	}

	public class OrganizationAttributes
	{
		[JsonPropertyName("name")]
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
