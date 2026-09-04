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
	public class CertificateProductData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public CertificateProduct CertificateProduct { get; set; }
	}

	public class CertificateProduct
	{
		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public CertificateProductAttributes Attributes { get; set; }
	}

	public class CertificateProductAttributes
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
