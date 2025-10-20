using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetAll();
        Task<StudentDTO> GetById(int id);
        Task<StudentDTO> Add(CreateStudDTO student);
        Task<StudentDTO> Update(int id, UpdateStudDTO student);
        Task<StudentDTO> Delete(int id);
    }
}
