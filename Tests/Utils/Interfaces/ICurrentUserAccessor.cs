using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Utils.Interfaces
{
    public interface ICurrentUserAccessor
    {
        public string GetCurrentUsername();
    }
}
