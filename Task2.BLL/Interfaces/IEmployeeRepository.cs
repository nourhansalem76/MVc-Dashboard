using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.DAL.Models;

namespace Task2.BLL.Interfaces
{
    public interface IEmployeeRepository: IGenericRepository<Employee>
    {
      IQueryable<Employee> GetEmployeeByAddress(string address);
      IQueryable<Employee> GetByName(string name);
    }
}
