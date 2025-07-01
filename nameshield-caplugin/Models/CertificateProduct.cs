using Newtonsoft.Json;

using Org.BouncyCastle.Tls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class CertificateProductData
	{
		[JsonProperty("data")]
		public CertificateProduct CertificateProduct { get; set; }
	}

	public class CertificateProduct
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public CertificateProductAttributes Attributes { get; set; }
	}

	public class CertificateProductAttributes
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
