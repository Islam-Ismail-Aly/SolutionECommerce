using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketoo.Application.Interfaces
{
    public interface ICommonRepository<T> where T : class
    {
        Task<int> CountAsync();
    }
}
