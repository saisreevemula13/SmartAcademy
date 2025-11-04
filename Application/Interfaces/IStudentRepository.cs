using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync(string? filterOn=null, string? filterQuery=null);
        Task<Student> GetByIdAsync(int id);
        Task<Student> AddAsync(Student student);
        Task<Student> UpdateAsync(int id, Student student);
        Task<Student> DeleteAsync(int id);
    }
}
