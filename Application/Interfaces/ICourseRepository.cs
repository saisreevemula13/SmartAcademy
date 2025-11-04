using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync(string? filterOn=null, string? filterQuery = null, string ? sortBy = null,
            bool? isAscending = null,
             int pageNumber = 1,int pageSize = 1000);
        Task<Course> GetByIdAsync(int id);
        Task<Course> AddAsync(Course course);
        Task<Course> UpdateAsync(int id, Course course);
        Task<Course> DeleteAsync(int id);

    }
}
