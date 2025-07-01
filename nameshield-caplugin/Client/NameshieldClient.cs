using Keyfactor.Extensions.CAPlugin.Nameshield.API;
using Keyfactor.Extensions.CAPlugin.Nameshield.Models;
using Keyfactor.Logging;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Org.BouncyCastle.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.Client
{
	public class NameshieldClient
	{
		private static ILogger Logger => LogHandler.GetClassLogger<NameshieldClient>();

		private HttpClient RestClient { get; }

		public NameshieldClient(HttpClient client)
		{
			RestClient = client;
		}

		public async Task<ListOrganizationsResponse> ListOrganizations()
		{
			var response = await RestClient.GetAsync("ssl/v2/organizations");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Organizations response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<OrganizationData> (responseContent);
				return new ListOrganizationsResponse { Organizations = responseObj.Organizations };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving organizations: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<ListProductsResponse> ListProducts()
		{
			var response = await RestClient.GetAsync("ssl/v2/products");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Products response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<ProductsData>(responseContent);
				return new ListProductsResponse { Products = responseObj.Products };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving product list: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<ListCertificatesResponse> ListCertificates()
		{
			var response = await RestClient.GetAsync("ssl/v2/certificates?fields[certificate]=status,serial,pem");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Certificates response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<CertificatesData>(responseContent);
				return new ListCertificatesResponse { Certificates = responseObj.Certificates };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving certificate list: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<GetCertificateResponse> GetCertificate(string ID)
		{
			var response = await RestClient.GetAsync($"ssl/v2/certificates/{ID}?fields[certificate]=status,serial,pem");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Certificate response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<CertificateData>(responseContent);
				return new GetCertificateResponse { Certificate = responseObj.Certificate };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving certificate: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<GetCertificateProductResponse> GetCertificateProduct(string ID)
		{
			var response = await RestClient.GetAsync($"ssl/v2/certificates/{ID}/product");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Certificate Product response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<CertificateProductData>(responseContent);
				return new GetCertificateProductResponse { CertificateProduct = responseObj.CertificateProduct };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving certificate product: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<RequestCertificateResponse> RequestCertificate(CertificateRequest req)
		{
			var response = await RestClient.PostAsJsonAsync("ssl/v2/orders", req);
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"POST Order response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<OrderData>(responseContent); ;
				return new RequestCertificateResponse { Order = responseObj.Order };
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error requesting certificate: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public async Task<bool> RevokeCertificate(string serialNum)
		{
			var response = await RestClient.PostAsync($"ssl/v2/certificates/{serialNum}/revoke", null);
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"POST Revoke response: {responseContent}");
				return true;
			}
			else
			{
				var error = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error revoking certificate: {error.Error.Code} | {error.Error.Message}");
			}
		}

		public static NameshieldClient InitializeClient(NameshieldConfig config)
		{
			Logger.MethodEntry(LogLevel.Debug);

			string apiEndpoint = config.ApiUrl;
			if (!apiEndpoint.EndsWith("/"))
			{
				apiEndpoint += "/";
			}

			HttpClient restClient = new HttpClient()
			{
				BaseAddress = new Uri(apiEndpoint)
			};

			restClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", $"{config.ApiToken}");

			Logger.MethodExit(LogLevel.Debug);
			return new NameshieldClient(restClient);
		}
	}
}
