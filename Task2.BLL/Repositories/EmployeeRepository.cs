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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
       

        public EmployeeRepository(MVCAppDbContext dbContext):base(dbContext)
        {
            
        }

        public IQueryable<Employee> GetByName(string name)
        {
          return  _dbContext.Employees.Where(E => E.Name.ToLower().Contains(name.ToLower()));
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        {
           throw new NotImplementedException();
        }

    }
}
