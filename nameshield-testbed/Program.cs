// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class Program
{
	public static async Task Main(string[] args)
	{
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "21cee8c3-b472-443e-ba47-1fbac18d7b7e");

			ListOrgs(client);
			//OrderCertificate(client);
		}
		Console.ReadLine();
		Console.WriteLine("Hello, World!");
	}

	public static void ListOrgs(HttpClient client)
	{
		var response = client.GetAsync("https://ote-api.nameshield.net/ssl/v2/organizations").Result;
		if (response.IsSuccessStatusCode)
		{
			var responseBody = response.Content.ReadAsStringAsync().Result;
			
			var responseObj = JsonConvert.DeserializeObject<OrganizationData>(responseBody);
			Console.WriteLine(responseBody);
		}
		else
		{
			Console.WriteLine($"Error: {response.StatusCode}");
			var errorContent = response.Content.ReadAsStringAsync().Result;
			var errorObj = JsonConvert.DeserializeObject<ErrorData>(errorContent);
			Console.WriteLine(errorContent);
		}
	}


	public class ErrorData
	{
		[JsonProperty("errors")]
		public Error Error { get; set; }
	}
	public class Error
	{
		[JsonProperty("code")]
		public string Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
	}

	public class OrganizationData
	{
		[JsonProperty("data")]
		public List<Organization> Organizations { get; set; }
	}

	public class Organization
	{
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("attributes")]
		public OrganizationAttributes Attributes { get; set; }
	}

	public class OrganizationAttributes
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public static async void OrderCertificate(HttpClient client)
	{
		var orderData = new JObject
		{
			["data"] = new JObject
			{
				["type"] = "order",
				["attributes"] = new JObject
				{
					["action"] = "create",
					["csr"] = "-----BEGIN CERTIFICATE REQUEST-----MIICdjCCAV4CAQAwMTESMBAGA1UECgwJS2V5ZmFjdG9yMRswGQYDVQQDDBJrZXlmYWN0b3IudGVzdC5jb20wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCENsFrFWrbrrVOr5XcO+yGBo4i5lGTyGNIqs1Thd0XwSRNXEHIKeQQsMbFK+vBGBCt66g6+5I60MkYbqItM5B5JlQJ79ChZu/Bb1WSNObf9Ie0T7olPUEUNecvjeHIREVWO1XdIM/eDkXIFfJykf0wJM1oAQbmhUVD2MYcbTCnc5GDWjWA2W3D2wHaK5eX8oTaEG/KwS9IttIxpuIHKyHlrPGDNzUrO3Ztbk2YyJ7EW6jJoIyzZpNQtQ+o5JVjfD5Q4vrPokmWolR2B+NwzmkNB2fA538UpZZGaedVTYHgKSpUCJGIJ+TSiywO784/LmWt52ji4452BmW6XXlPb5m3AgMBAAGgADANBgkqhkiG9w0BAQsFAAOCAQEAOr+RROgM7YEMECD9ukTQ7uFjs09wEP8ZLXbgAlscnGtQzINjJkawCyx7CidQZz0FISBdc1YjUgBCKzfHiecYrR1FpHi+HwuvK1obbxqDz3xKZ3GW8paeNyXq6wFtAGKF4DSpeTpOnkg1Ydlh3vegT5FOXakA3g51wiRy9LquV0jNb8yL9FbvhOLG/XRDcAZ45rH9S9BwMY95o7xsmOOcZVcuCsHaQGFPM5h8VgQqAOLeRMC1tpzCRqg12yp+bbHd6yb4nNiAPSm1o+2iItDfkA+7kbA7o04JDRhFN+Oy/m5IHzyDStnhgENja3duVnK1nevLNj+pdKDm6n9FIieGjw==-----END CERTIFICATE REQUEST-----",
					["duration"] = "2 years",
				},
				["relationships"] = new JObject
				{
					["product"] = new JObject
					{
						["data"] = new JObject
						{
							["type"] = "product",
							["id"] = "11"
						}
					},
					["organization"] = new JObject
					{
						["data"] = new JObject
						{
							["type"] = "organization",
							["id"] = "BSVBG"
						}
					}
				}
			}
		};

		var content = new StringContent(orderData.ToString(), Encoding.UTF8, "application/json");
		Console.WriteLine(content.ReadAsStringAsync().Result);
		var response = await client.PostAsync("https://ote-api.nameshield.net/ssl/v2/orders", content);

		if (response.IsSuccessStatusCode)
		{
			var responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseBody);

		}
		else
		{
			Console.WriteLine($"Error: {response.StatusCode}");
			var errorContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(errorContent);
		}
	}
}
