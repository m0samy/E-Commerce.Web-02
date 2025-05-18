using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace DomainLayer.Contracts
{
    public interface ISpecifications<TEntity , TKey> where TEntity : BaseEntity<TKey>
    {
        //Property Signature for each Dynamic part in Query
        //الجزء اللي هيبقى داينمك بتاع ال Expression
        public Expression<Func<TEntity , bool>>? Criteria { get; }
        public List<Expression<Func<TEntity , object>>> IncludeExpressions { get; }
        public Expression<Func<TEntity , object>> OrderBy { get; }
        public Expression<Func<TEntity , object>> OrderByDescending { get; }

        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; set; }
    }
}
