using System;
using System.Linq.Expressions;

namespace DomainDriven.Specifications
{
    public class AdHocSpecification<T> : Specification<T>
    {
        public AdHocSpecification(Expression<Func<T, bool>> expression) : base(expression)
        {
        }
    }
}