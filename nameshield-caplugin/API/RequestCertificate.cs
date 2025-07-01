using Keyfactor.Extensions.CAPlugin.Nameshield.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.API
{
	public class RequestCertificateResponse
	{
		public Order Order { get; set; }
	}

	public class CertificateRequest
	{
		public CertificateRequest()
		{
			data = new CertificateRequestData();
			data.attributes = new CertificateRequestAttributes();
			data.relationships = new CertificateRequestRelationships();
			data.relationships.product = new CertRequestRel();
			data.relationships.product.data = new RelationshipData();
			data.relationships.organization = new CertRequestRel();
			data.relationships.organization.data = new RelationshipData();
		}

		[JsonProperty("data")]
		public CertificateRequestData data { get; set; }
	}

	public class CertificateRequestData
	{
		[JsonProperty("type")]
		public string type { get; set; }

		[JsonProperty("attributes")]
		public CertificateRequestAttributes attributes { get; set; }

		[JsonProperty("relationships")]
		public CertificateRequestRelationships relationships { get; set; }
	}

	public class CertificateRequestAttributes
	{
		[JsonProperty("action")]
		public string action { get; set; }
		[JsonProperty("csr")]
		public string csr { get; set; }
		[JsonProperty("duration")]
		public string duration { get; set; }
	}

	public class CertificateRequestRelationships
	{
		[JsonProperty("product")]
		public CertRequestRel product { get; set; }
		[JsonProperty("organization")]
		public CertRequestRel organization { get; set; }
	}

	public class CertRequestRel
	{
		[JsonProperty("data")]
		public RelationshipData data { get; set; }
	}


}
