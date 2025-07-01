using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield
{
	public static class Constants
	{
		public class Config
		{
			public class CA
			{
				public const string API_URL = "ApiUrl";
				public const string API_TOKEN = "ApiToken";
				public const string ENABLED = "Enabled";
			}
			public class Template
			{
				public const string ORGANIZATION_ID = "OrganizationId";
				public const string ORGANIZATION = "Organization";
			}
		}
	}
}
