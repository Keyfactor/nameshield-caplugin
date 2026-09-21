using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class ProductsData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public List<Product> Products { get; set; }
	}

	public class Product
	{
		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public ProductAttributes Attributes { get; set; }
	}

	public class ProductAttributes
	{
		[JsonPropertyName("name")]
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonPropertyName("can_be_ordered")]
		[JsonProperty("can_be_ordered")]
		public bool CanBeOrdered { get; set; }
		[JsonPropertyName("accept_reissue")]
		[JsonProperty("accept_reissue")]
		public bool CanReissue { get; set; }
		[JsonPropertyName("allowed_durations")]
		[JsonProperty("allowed_durations")]
		public List<string> Durations { get; set; }
	}
}