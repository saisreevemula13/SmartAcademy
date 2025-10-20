using Application.DTO;

namespace Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDTO>> GetAllEnrollments();
        Task<EnrollmentDTO> GetEnrollmentById(int id);
        Task<EnrollmentDTO> DeleteById(int id);
        Task<EnrollmentDTO> UpdateEnrollment(int id, UpdateEnrollmentDTO dto);
        Task<EnrollmentDTO> CreateEnrollment(CreateEnrollmentDTO dto);
    }
}
