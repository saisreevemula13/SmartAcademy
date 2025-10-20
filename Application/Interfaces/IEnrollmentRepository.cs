using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment> GetByIdAsync(int id);
        Task<Enrollment> UpdateByIdAsync(int id, Enrollment enrollment);
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<Enrollment> DeleteByIdAsync(int id);
    }
}
