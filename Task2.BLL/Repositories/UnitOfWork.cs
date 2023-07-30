using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.BLL.Interfaces;
using Task2.DAL.Contexts;

namespace Task2.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MVCAppDbContext _dbContext;

        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set ; }

        public UnitOfWork(MVCAppDbContext dbContext)
        {
            EmployeeRepository= new EmployeeRepository(dbContext);
            DepartmentRepository= new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> Complete()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        
          =>  _dbContext.Dispose();
        
    }
}
