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
using System.Text.Json;
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
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving organizations: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
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
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving product list: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
			}
		}

		public async Task<ListCertificatesResponse> ListCertificates()
		{
			string url = "ssl/v2/certificates?fields[certificate]=status,serial,pem";
			var response = await RestClient.GetAsync(url);
			List<Certificate> certList = new List<Certificate>();
			do
			{
				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					Logger.LogTrace($"GET Certificates response: {responseContent}");
					var responseObj = JsonConvert.DeserializeObject<CertificatesData>(responseContent);
					certList.AddRange(responseObj.Certificates);
					if (!string.IsNullOrEmpty(responseObj.Links.Next))
					{
						Uri uri = new Uri(responseObj.Links.Next);
						url = uri.PathAndQuery.Substring(1); // remove the leading /
						response = await RestClient.GetAsync(url);
					}
					else
					{
						url = null;
					}
				}
				else
				{
					var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
					throw new Exception($"Error retrieving certificate list: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
				}
			} while (!string.IsNullOrEmpty(url));
			return new ListCertificatesResponse { Certificates = certList };
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
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving certificate: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
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
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error retrieving certificate product: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
			}
		}

		public async Task<RequestCertificateResponse> RequestCertificate(CertificateRequest req)
		{
			var body = req.GetOrderData();
			var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
			var response = await RestClient.PostAsync("ssl/v2/orders", content);
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"POST Order response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<OrderData>(responseContent); ;
				return new RequestCertificateResponse { Order = responseObj.Order };
			}
			else
			{
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error requesting certificate: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
			}
		}

		public async Task<RequestCertificateResponse> GetOrderDetails(string reqId)
		{
			var response = await RestClient.GetAsync($"ssl/v2/orders/{reqId}");
			if (response.IsSuccessStatusCode)
			{
				string responseContent = await response.Content.ReadAsStringAsync();
				Logger.LogTrace($"GET Order Details response: {responseContent}");
				var responseObj = JsonConvert.DeserializeObject<OrderData>(responseContent);
				return new RequestCertificateResponse { Order = responseObj.Order };
			}
			else
			{
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error pulling order details: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
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
				var errors = JsonConvert.DeserializeObject<ErrorData>(await response.Content.ReadAsStringAsync());
				throw new Exception($"Error revoking certificate: {errors.Errors[0].Title} | {errors.Errors[0].Detail}");
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
