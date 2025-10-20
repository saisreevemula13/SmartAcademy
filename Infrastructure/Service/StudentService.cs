using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<List<StudentDTO>> GetAll()
        {
            var students = await _studentRepository.GetAllAsync();
            return _mapper.Map<List<StudentDTO>>(students);
        }

        public async Task<StudentDTO> GetById(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
                throw new NotFoundException($"Student with ID {id} not found");

            return _mapper.Map<StudentDTO>(student);
        }
        
        public async Task<StudentDTO> Add(CreateStudDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var student = _mapper.Map<Student>(dto);
            student.PasswordHash = HashPassword(dto.Password); // hash password

            student = await _studentRepository.AddAsync(student);
            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> Update(int id, UpdateStudDTO dto)
        {
            // hello
            var student = _mapper.Map<Student>(dto);
            student = await _studentRepository.UpdateAsync(id, student)
                ?? throw new NotFoundException($"Student with ID {id} not found to update.");

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> Delete(int id)
        {

            var student = await _studentRepository.DeleteAsync(id)
           ?? throw new NotFoundException($"Student with ID {id} not found to delete.");

            return _mapper.Map<StudentDTO>(student);
        }

        //  Password hashing helper
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
