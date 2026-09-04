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
	public class CertificateData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public Certificate Certificate { get; set; }
	}

	public class LinkData
	{
		[JsonPropertyName("next")]
		[JsonProperty("next")]
		public string Next { get; set; }
	}

	public class CertificatesData
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public List<Certificate> Certificates { get; set; }

		[JsonPropertyName("links")]
		[JsonProperty("links")]
		public LinkData Links { get; set; }
	}

	public class Certificate
	{
		[JsonPropertyName("id")]
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public CertificateAttributes Attributes { get; set; }
	}

	public class CertificateAttributes
	{
		[JsonPropertyName("status")]
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonPropertyName("serial")]
		[JsonProperty("serial")]
		public string Serial { get; set; }

		[JsonPropertyName("pem")]
		[JsonProperty("pem")]
		public string Pem { get; set; }
	}
}
