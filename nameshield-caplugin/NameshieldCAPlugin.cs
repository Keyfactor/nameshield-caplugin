using Keyfactor.AnyGateway.Extensions;
using Keyfactor.Extensions.CAPlugin.Nameshield.API;
using Keyfactor.Extensions.CAPlugin.Nameshield.Client;
using Keyfactor.Logging;
using Keyfactor.PKI.Enums.EJBCA;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield
{
	public class NameshieldCAPlugin : IAnyCAPlugin
	{
		private NameshieldConfig _config;
		private readonly ILogger _logger;
		private ICertificateDataReader _certificateDataReader;

		public NameshieldCAPlugin()
		{
			_logger = LogHandler.GetClassLogger<NameshieldCAPlugin>();
		}

		public void Initialize(IAnyCAPluginConfigProvider configProvider, ICertificateDataReader certificateDataReader)
		{
			_certificateDataReader = certificateDataReader;
			string rawConfig = JsonConvert.SerializeObject(configProvider.CAConnectionData);
			_config = JsonConvert.DeserializeObject<NameshieldConfig>(rawConfig);
		}

		public async Task<EnrollmentResult> Enroll(string csr, string subject, Dictionary<string, string[]> san, EnrollmentProductInfo productInfo, RequestFormat requestFormat, EnrollmentType enrollmnentType)
		{
			_logger.MethodEntry(LogLevel.Debug);
			NameshieldClient client = NameshieldClient.InitializeClient(_config);

			var allProducts = Task.Run(async () => await client.ListProducts()).Result;
			_logger.LogTrace($"Found {allProducts.Products.Count} products");
			string productId = null;
			foreach (var product in allProducts.Products)
			{
				if (string.Equals(product.Attributes.Name, productInfo.ProductID))
				{
					_logger.LogTrace($"Found {productInfo.ProductID} product, using ID {product.Id}");
					productId = product.Id;
					break;
				}
			}
			if (string.IsNullOrEmpty(productId))
			{
				throw new Exception($"Cannot find product type {productInfo.ProductID} for enrollment");
			}

			string orgId = null;
			if (productInfo.ProductParameters.ContainsKey(Constants.Config.Template.ORGANIZATION_ID) && !string.IsNullOrEmpty(productInfo.ProductParameters[Constants.Config.Template.ORGANIZATION_ID]))
			{
				orgId = productInfo.ProductParameters[Constants.Config.Template.ORGANIZATION_ID];
				_logger.LogTrace($"Using organization ID {orgId}");
			}

			if (string.IsNullOrEmpty(orgId))
			{
				string orgName = null;
				if (productInfo.ProductParameters.ContainsKey(Constants.Config.Template.ORGANIZATION) && !string.IsNullOrEmpty(productInfo.ProductParameters[Constants.Config.Template.ORGANIZATION]))
				{
					orgName = productInfo.ProductParameters[Constants.Config.Template.ORGANIZATION];
				}
				else
				{
					orgName = ParseSubject(subject, "O=");
				}
				var organizations = Task.Run(async () => await client.ListOrganizations()).Result;
				_logger.LogTrace($"Found {organizations.Organizations.Count} organizations");
				foreach (var organization in organizations.Organizations)
				{
					if (string.Equals(organization.Attributes.Name, orgName, StringComparison.OrdinalIgnoreCase))
					{
						_logger.LogTrace($"Found organization with name {orgName}, using ID {organization.Id}");
						orgId = organization.Id;
						break;
					}
				}
			}

			if (string.IsNullOrEmpty(orgId))
			{
				throw new Exception($"No organization provided in attributes or subject that matches an organization in the Nameshield account");
			}

			CertificateRequest request = new CertificateRequest();
			request.data.type = "order";
			request.data.attributes.action = "create";
			request.data.attributes.csr = csr;
			request.data.attributes.duration = "1 year";
			request.data.relationships.product.data.type = "product";
			request.data.relationships.product.data.id = productId;
			request.data.relationships.organization.data.type = "organization";
			request.data.relationships.organization.data.id = orgId;

			var response = Task.Run(async () => await client.RequestCertificate(request)).Result;

			var status = response.Order.Attributes.Status;

			if (string.Equals(status, "rejected", StringComparison.OrdinalIgnoreCase))
			{
				throw new Exception($"Certificate request for subect {subject} was rejected");
			}
			int time = 1;
			while (string.Equals(status, "checked_out", StringComparison.OrdinalIgnoreCase) && time <= 10)
			{
				_logger.LogTrace($"Cert request returned CHECKED_OUT status, rechecking in 5 seconds. Pickup attempt {time} of 10");
				// Sleep for 5 seconds then try again, up to a max of 10 tries
				Thread.Sleep(5000);
				time++;
				response = Task.Run(async () => await client.GetOrderDetails(response.Order.Id)).Result;
				status = response.Order.Attributes.Status;
			}

			if (!string.Equals(status, "delivered", StringComparison.OrdinalIgnoreCase))
			{
				_logger.LogTrace($"Cert request submitted successfully but not delivered, will be picked up by a fugure sync once it is issued.");
				return new EnrollmentResult
				{
					CARequestID = response.Order.Id,
					Status = (int)EndEntityStatus.EXTERNALVALIDATION,
					StatusMessage = "Certificate is pending issuance and will be picked up by a future sync."
				};
			}
			_logger.LogTrace($"Certificate enrolled successfully");

			var cert = GetSingleRecord(response.Order.Relationships.Certificate.Data.id).Result;

			return new EnrollmentResult
			{
				CARequestID = cert.CARequestID,
				Certificate = cert.Certificate,
				Status = cert.Status,
				StatusMessage = $"Successfully enrolled for certificate {subject}"
			};
		}
		
		public Dictionary<string, PropertyConfigInfo> GetCAConnectorAnnotations()
		{
			return new Dictionary<string, PropertyConfigInfo>()
			{
				[Constants.Config.CA.API_URL] = new PropertyConfigInfo()
				{
					Comments = "The base URL to send API requests to. Standard URLs are https://api.nameshield.net/ for production environment, and https://ote-api.nameshield.net/ for test and development",
					Hidden = false,
					DefaultValue = "https://ote-api.nameshield.net/",
					Type = "String"
				},
				[Constants.Config.CA.API_TOKEN] = new PropertyConfigInfo()
				{
					Comments = "The Bearer token to use to authenticate to the Nameshield API",
					Hidden = false,
					DefaultValue = "",
					Type = "String"
				},
				[Constants.Config.CA.ENABLED] = new PropertyConfigInfo()
				{
					Comments = "Flag to Enable or Disable gateway functionality. Diabling is primarily used to allow creation of the CA prior to configuration information being available.",
					Hidden = false,
					DefaultValue = true,
					Type = "Boolean"
				}
			};
		}

		public List<string> GetProductIds()
		{
			NameshieldClient client = NameshieldClient.InitializeClient(_config);

			var productList = Task.Run(async () => await client.ListProducts()).Result;

			List<string> productIDs = new List<string>();
			foreach (var product in productList.Products)
			{
				productIDs.Add(product.Attributes.Name);
			}
			return productIDs;
		}

		public async Task<AnyCAPluginCertificate> GetSingleRecord(string caRequestID)
		{
			NameshieldClient client = NameshieldClient.InitializeClient(_config);

			var cert = Task.Run(async () => await client.GetCertificate(caRequestID)).Result;
			var certProduct = Task.Run(async () => await client.GetCertificateProduct(caRequestID)).Result;

			int status = ConvertToKeyfactorStatus(cert.Certificate.Attributes.Status, caRequestID);

			return new AnyCAPluginCertificate()
			{
				CARequestID = caRequestID,
				ProductID = certProduct.CertificateProduct.Attributes.Name,
				Status = status,
				Certificate = cert.Certificate.Attributes.Pem,
				RevocationDate = (status == (int)EndEntityStatus.REVOKED) ? DateTime.UtcNow : null
			};
		}

		public Dictionary<string, PropertyConfigInfo> GetTemplateParameterAnnotations()
		{
			return new Dictionary<string, PropertyConfigInfo>()
			{
				[Constants.Config.Template.ORGANIZATION_ID] = new PropertyConfigInfo()
				{
					Comments = "If you know your organization's ID in nameshield, you can supply that here and that will be used as the organization regardless of what values exist in the request or the Organization field",
					Hidden = false,
					DefaultValue = "",
					Type = "String"
				},
				[Constants.Config.Template.ORGANIZATION] = new PropertyConfigInfo()
				{
					Comments = "If OrganizationId is empty, and an organization name is provided here, the Nameshield gateway will use that name to do organization lookups when enrolling. If neither OrganizationId nor Organization is supplied, the gateway will use whatever is in the O= field of the request subject.",
					Hidden = false,
					DefaultValue = "",
					Type = "String"
				}
			};
		}

		public async Task Ping()
		{
			_logger.MethodEntry(LogLevel.Debug);
			if (!_config.Enabled)
			{
				_logger.LogWarning($"The CA is currently in the Disabled state. It must be Enabled to perform operations. Skipping connectivity test...");
				_logger.MethodExit(LogLevel.Debug);
			}

			try
			{
				_logger.LogDebug("Attempting to ping Nameshield API");
				var client = NameshieldClient.InitializeClient(_config);
				_ = Task.Run(async () => await client.ListOrganizations()).Result;
			}
			catch (Exception ex)
			{
				_logger.LogError($"There was an error contacting Nameshield: {ex.Message}");
				throw new Exception($"There was an error contacting Nameshield: {ex.Message}", ex);
			}
		}

		public async Task<int> Revoke(string caRequestID, string hexSerialNumber, uint revocationReason)
		{
			NameshieldClient client = NameshieldClient.InitializeClient(_config);

			var response = Task.Run(async () => await client.RevokeCertificate(hexSerialNumber)).Result;

			if (response)
			{
				return (int)EndEntityStatus.REVOKED;
			}
			return -1;
		}

		public async Task Synchronize(BlockingCollection<AnyCAPluginCertificate> blockingBuffer, DateTime? lastSync, bool fullSync, CancellationToken cancelToken)
		{
			NameshieldClient client = NameshieldClient.InitializeClient(_config);

			var certs = Task.Run(async () => await client.ListCertificates()).Result;

			foreach (var cert in certs.Certificates)
			{
				var certProduct = Task.Run(async () => await client.GetCertificateProduct(cert.Id)).Result;
				int status = ConvertToKeyfactorStatus(cert.Attributes.Status, cert.Id);
				var newCert = new AnyCAPluginCertificate()
				{
					CARequestID = cert.Id,
					ProductID = certProduct.CertificateProduct.Attributes.Name,
					Status = status,
					Certificate = cert.Attributes.Pem,
					RevocationDate = (status == (int)EndEntityStatus.REVOKED) ? DateTime.UtcNow : null
				};

				blockingBuffer.Add(newCert);
			}
			blockingBuffer.CompleteAdding();
		}

		public async Task ValidateCAConnectionInfo(Dictionary<string, object> connectionInfo)
		{
			_logger.MethodEntry(LogLevel.Debug);
			List<string> errors = new List<string>();
			errors.Add(ValidateConfigurationKey(connectionInfo, Constants.Config.CA.API_URL));
			errors.Add(ValidateConfigurationKey(connectionInfo, Constants.Config.CA.API_TOKEN));

			_logger.MethodExit(LogLevel.Debug);
			if (errors.Any(s => !string.IsNullOrEmpty(s)))
			{
				throw new Exception(string.Join("|", errors.All(s => !string.IsNullOrEmpty(s))));
			}
		}

		public async Task ValidateProductInfo(EnrollmentProductInfo productInfo, Dictionary<string, object> connectionInfo)
		{
			_logger.MethodEntry(LogLevel.Debug);
			string rawConfig = JsonConvert.SerializeObject(connectionInfo);
			var parsedConfig = JsonConvert.DeserializeObject<NameshieldConfig>(rawConfig);
			NameshieldClient localClient = NameshieldClient.InitializeClient(parsedConfig);

			var productList = Task.Run(async () => await localClient.ListProducts()).Result;

			bool foundProduct = false;
			foreach (var product in productList.Products)
			{
				if (string.Equals(product.Attributes.Name, productInfo.ProductID, StringComparison.OrdinalIgnoreCase))
				{
					foundProduct = true;
					break;
				}
			}

			if (!foundProduct)
			{
				throw new Exception($"Product ID {productInfo.ProductID} was not found in the list of valid products.");
			}
		}

		private static string ValidateConfigurationKey(Dictionary<string, object> connectionInfo, string key)
		{
			if (!connectionInfo.TryGetValue(key, out object tempValue) && tempValue != null)
			{
				return $"{key} is a required configuration value";
			}

			return string.Empty;
		}

		private static int ConvertToKeyfactorStatus(string status, string sslId)
		{
			switch (status.ToUpper())
			{
				case "ACTIVE":
				case "EXPIRED":
				case "DO_NOT_RENEW":
				case "TRANSFERRED":
					return (int)EndEntityStatus.GENERATED;

				case "WAITING":
					return (int)EndEntityStatus.EXTERNALVALIDATION;

				case "REVOKED":
					return (int)EndEntityStatus.REVOKED;

				default:
					throw new Exception($"Request ID {sslId} has unknown status {status}");
			}
		}

		private static string ParseSubject(string subject, string rdn, bool required = true)
		{
			string escapedSubject = subject.Replace("\\,", "|");
			string rdnString = escapedSubject.Split(',').ToList().Where(x => x.Contains(rdn)).FirstOrDefault();

			if (!string.IsNullOrEmpty(rdnString))
			{
				return rdnString.Replace(rdn, "").Replace("|", ",").Trim();
			}
			else if (required)
			{
				throw new Exception($"The request is missing a {rdn} value");
			}
			else
			{
				return null;
			}
		}
	}
}
