using Application.DTO;

namespace Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentDTO>> GetAllEnrollments(string? filterOn = null, string? filterQuery = null, string? sortBy=null, Boolean? isAscending=true,
            int pageNumber=1, int pageSize=1000);
        Task<EnrollmentDTO> GetEnrollmentById(int id);
        Task<EnrollmentDTO> DeleteById(int id);
        Task<EnrollmentDTO> UpdateEnrollment(int id, UpdateEnrollmentDTO dto);
        Task<EnrollmentDTO> CreateEnrollment(CreateEnrollmentDTO dto);
    }
}
