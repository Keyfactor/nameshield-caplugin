using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class ProductsData
	{
		[JsonProperty("data")]
		public List<Product> Products { get; set; }
	}

	public class Product
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public ProductAttributes Attributes { get; set; }
	}

	public class ProductAttributes
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("can_be_ordered")]
		public bool CanBeOrdered { get; set; }
		[JsonProperty("accept_reissue")]
		public bool CanReissue { get; set; }
		[JsonProperty("allowed_durations")]
		public List<string> Durations { get; set; }
	}
}