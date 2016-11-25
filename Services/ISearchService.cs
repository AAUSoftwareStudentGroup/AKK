using System.Collections.Generic;

namespace AKK.Services
{
    public interface ISearchService<T>
    {
        IEnumerable<T> Search(string searchStr);
    }
}