using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.BLL.Interfaces;
using Task2.DAL.Contexts;
using Task2.DAL.Models;

namespace Task2.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly MVCAppDbContext _dbContext;
        public GenericRepository(MVCAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(T item)
        
         => await  _dbContext.Set<T>().AddAsync(item);



        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);

        }

        public async Task< IEnumerable<T> >GetAll()
        {
            if(typeof(T)==typeof(Employee))
             return  (IEnumerable<T>)await _dbContext.Employees.Include(E => E.Department).ToListAsync();
            
            else
             return await _dbContext.Set<T>().ToListAsync();
        }
          

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        
         =>   _dbContext.Set<T>().Update(item);

      
    }
}
