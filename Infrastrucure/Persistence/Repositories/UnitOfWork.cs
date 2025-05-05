using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Contracts;
using DomainLayer.Models;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext _dbContext) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = []; 
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            //Get Type Name بتاع TEntity 
            var typeName = typeof(TEntity).Name;
            //Dictionary<string,object> هيبقى عندي هعرفه عشان بشوف التايب اللي جبته موجود ولا لأ=>
            //string key [Name of type] , object object from Generic Repository

            //if(_repositories.ContainsKey(typeName)) 
            //    return (IGenericRepository<TEntity , TKey>) _repositories[typeName];

            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity , TKey>) value;
            else
            {
                //Create Object from GenericRepositorty
                var Repo = new GenericRepository<TEntity, TKey>(_dbContext);
                //Store Object in Dictionary
                _repositories.Add(typeName, Repo);
                //Return object
                return Repo;
            }

        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        
    }
}
