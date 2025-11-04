using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAll(string? filterOn=null,string? filterQuery=null,
            string? sortBy = null,
            bool? isAscending = null,
             int pageNumber = 1,int pageSize = 1000);
        Task<CourseDTO> GetById(int id);
        Task<CourseDTO> Add(CreateCourseDTO student);
        Task<CourseDTO> Update(int id, UpdateCourseDTO student);
        Task<CourseDTO> Delete(int id);
    }
}
