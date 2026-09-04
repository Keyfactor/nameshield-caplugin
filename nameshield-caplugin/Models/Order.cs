using Newtonsoft.Json;

using Org.BouncyCastle.Tls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class OrderData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public Order Order { get; set; }
	}

	public class Order
	{
		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public OrderAttributes Attributes { get; set; }
		[JsonPropertyName("relationships")]
		[JsonProperty("relationships")]
		public OrderRelationships Relationships { get; set; }
	}

	public class OrderAttributes
	{
		[JsonPropertyName("status")]
		[JsonProperty("status")]
		public string Status { get; set; }
	}

	public class OrderRelationships
	{
		[JsonPropertyName("certificate")]
		[JsonProperty("certificate")]
		public OrderCertRelationship Certificate { get; set; }
	}

	public class OrderCertRelationship
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public RelationshipData Data { get; set; }
	}
}
