using Keyfactor.Extensions.CAPlugin.Nameshield.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

		public string GetOrderData()
		{
			var orderData = new
			{
				data = data
			};
			return System.Text.Json.JsonSerializer.Serialize(orderData);
		}


		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public CertificateRequestData data { get; set; }
	}

	public class CertificateRequestData
	{
		[JsonPropertyName("type")]
		[JsonProperty("type")]
		public string type { get; set; }

		[JsonPropertyName("attributes")]
		[JsonProperty("attributes")]
		public CertificateRequestAttributes attributes { get; set; }

		[JsonPropertyName("relationships")]
		[JsonProperty("relationships")]
		public CertificateRequestRelationships relationships { get; set; }
	}

	public class CertificateRequestAttributes
	{
		[JsonPropertyName("action")]
		[JsonProperty("action")]
		public string action { get; set; }
		[JsonPropertyName("csr")]
		[JsonProperty("csr")]
		public string csr { get; set; }
		[JsonPropertyName("duration")]
		[JsonProperty("duration")]
		public string duration { get; set; }
	}

	public class CertificateRequestRelationships
	{
		[JsonPropertyName("product")]
		[JsonProperty("product")]
		public CertRequestRel product { get; set; }
		[JsonPropertyName("organization")]
		[JsonProperty("organization")]
		public CertRequestRel organization { get; set; }
	}

	public class CertRequestRel
	{
		[JsonPropertyName("data")]
		[JsonProperty("data")]
		public RelationshipData data { get; set; }
	}


}
