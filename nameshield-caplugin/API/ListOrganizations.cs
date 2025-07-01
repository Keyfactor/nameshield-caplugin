using Keyfactor.Extensions.CAPlugin.Nameshield.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keyfactor.Extensions.CAPlugin.Nameshield.API
{
	public class ListOrganizationsResponse
	{
		public List<Organization> Organizations { get; set; }
	}
}
