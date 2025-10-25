using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;

        public StudentService(IStudentRepository studentRepository, IMapper mapper, ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<StudentDTO>> GetAll()
        {
            _logger.LogDebug("Getting all students from repository...");
            var students = await _studentRepository.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} students", students.Count);
            return _mapper.Map<List<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetById(int id)
        {
            _logger.LogDebug("Retrieving student with ID {Id}", id);
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student with ID {Id} not found", id);
                throw new NotFoundException($"Student with ID {id} not found");
            }

            _logger.LogInformation("Student {Id} retrieved successfully", id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> Add(CreateStudDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            _logger.LogInformation("Creating student {Name}", dto.Name);

            var student = _mapper.Map<Student>(dto);
            student.PasswordHash = HashPassword(dto.Password);

            student = await _studentRepository.AddAsync(student);

            _logger.LogInformation("Student created successfully: {Name} (ID {Id})", student.Name, student.Id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> Update(int id, UpdateStudDTO dto)
        {
            _logger.LogInformation("Updating student {Id}", id);
            var student = _mapper.Map<Student>(dto);

            student = await _studentRepository.UpdateAsync(id, student)
                ?? throw new NotFoundException($"Student with ID {id} not found for update.");

            _logger.LogInformation("Student {Id} updated successfully", id);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> Delete(int id)
        {
            _logger.LogInformation("Deleting student {Id}", id);

            var student = await _studentRepository.DeleteAsync(id)
                ?? throw new NotFoundException($"Student with ID {id} not found to delete.");

            _logger.LogWarning("Student {Id} deleted successfully", id);
            return _mapper.Map<StudentDTO>(student);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
