using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.LibraryExample.SsoServices.Abstractions;

public interface IFrontendService
{

	Task SignIn(string payload);
}
