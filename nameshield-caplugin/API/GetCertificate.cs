using Keyfactor.Extensions.CAPlugin.Nameshield.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Keyfactor.Extensions.CAPlugin.Nameshield.Models.CertificatesData;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.API
{
	public class GetCertificateResponse
	{
		public Certificate Certificate { get; set; }
	}
}
