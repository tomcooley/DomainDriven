using System.Collections.Generic;

namespace DomainDriven.Repository
{
    public interface IQuery<out T>
    {
        IEnumerable<T> Execute();
    }
}
