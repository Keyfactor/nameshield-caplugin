using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class OrganizationData
	{
		[JsonProperty("data")]
		public List<Organization> Organizations { get; set; }
	}

	public class Organization
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public OrganizationAttributes Attributes { get; set; }
	}

	public class OrganizationAttributes
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
