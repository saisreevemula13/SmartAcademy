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
        Task<IEnumerable<CourseDTO>> GetAll();
        Task<CourseDTO> GetById(int id);
        Task<CourseDTO> Add(CreateCourseDTO student);
        Task<CourseDTO> Update(int id, UpdateCourseDTO student);
        Task<CourseDTO> Delete(int id);
    }
}
