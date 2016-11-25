using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKK.Services
{
    public interface ISearchService<T>
    {
        IEnumerable<T> Search(string searchStr);
    }
}
