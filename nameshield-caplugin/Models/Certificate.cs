using Newtonsoft.Json;

using Org.BouncyCastle.Tls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Models
{
	public class CertificateData
	{
		[JsonProperty("data")]
		public Certificate Certificate { get; set; }
	}

	public class CertificatesData
	{
		[JsonProperty("data")]
		public List<Certificate> Certificates { get; set; }
	}

	public class Certificate
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public CertificateAttributes Attributes { get; set; }
	}

	public class CertificateAttributes
	{
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("serial")]
		public string Serial { get; set; }

		[JsonProperty("pem")]
		public string Pem { get; set; }
	}
}
