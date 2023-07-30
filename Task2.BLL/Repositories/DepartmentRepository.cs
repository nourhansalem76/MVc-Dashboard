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
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        
        public DepartmentRepository(MVCAppDbContext dbContext) :base(dbContext) 
        {
           
        }
        

    }
}
