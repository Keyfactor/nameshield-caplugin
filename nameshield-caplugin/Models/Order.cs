using Newtonsoft.Json;

using Org.BouncyCastle.Tls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class OrderData
	{
		[JsonProperty("data")]
		public Order Order { get; set; }
	}

	public class Order
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public OrderAttributes Attributes { get; set; }
		[JsonProperty("relationships")]
		public OrderRelationships Relationships { get; set; }
	}

	public class OrderAttributes
	{
		[JsonProperty("status")]
		public string Status { get; set; }
	}

	public class OrderRelationships
	{
		[JsonProperty("certificate")]
		public OrderCertRelationship Certificate { get; set; }
	}

	public class OrderCertRelationship
	{
		[JsonProperty("data")]
		public RelationshipData Data { get; set; }
	}
}
