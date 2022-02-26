using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuth.Core
{
    public interface ITemplateRepo
    {
        Task<Template> Get(string key);
    }
}
