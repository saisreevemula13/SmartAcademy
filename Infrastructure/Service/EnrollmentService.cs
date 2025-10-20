using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IMapper _mapper;
        public EnrollmentService(IEnrollmentRepository repository, IMapper mapper) 
        {
            _enrollmentRepository= repository;
            _mapper= mapper;
        }
        public async Task<EnrollmentDTO> CreateEnrollment(CreateEnrollmentDTO dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            if (dto.StudentId <= 0 || dto.CourseId <= 0)
                throw new ArgumentException("Invalid StudentId or CourseId");

            var enrollment=_mapper.Map<Enrollment>(dto);
            var student = await _enrollmentRepository.GetByIdAsync(dto.StudentId);
            if (student == null)
                throw new NotFoundException($"Student with ID {dto.StudentId} not found");

            var course = await _enrollmentRepository.GetByIdAsync(dto.CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID {dto.CourseId} not found");


            enrollment = await  _enrollmentRepository.CreateAsync(enrollment);
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }

        public async Task<EnrollmentDTO> DeleteById(int id)
        {
            var enrollment=await _enrollmentRepository.DeleteByIdAsync(id)
           ?? throw new NotFoundException($"Enrollment with ID {id} not found to delete.");
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }

        public async Task<IEnumerable<EnrollmentDTO>> GetAllEnrollments()
        {
            var enrollment = await _enrollmentRepository.GetAllAsync();
            if (enrollment == null)
                throw new NotFoundException($"Enrollments are not found to display");
            return _mapper.Map<IEnumerable<EnrollmentDTO>>(enrollment);
        }

        public async Task<EnrollmentDTO> GetEnrollmentById(int id)
        {
            var enrollment=await _enrollmentRepository.GetByIdAsync(id);
            if (enrollment == null)
                throw new NotFoundException($"Enrollment with ID {id} not found to display");
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }

        public async Task<EnrollmentDTO> UpdateEnrollment(int id, UpdateEnrollmentDTO dto)
        {
            var enrollment = _mapper.Map<Enrollment>(dto);
             enrollment = await _enrollmentRepository.UpdateByIdAsync(id, enrollment)??
                throw new NotFoundException($"Enrollment with ID {id} not found to update.");
            return _mapper.Map<EnrollmentDTO>(enrollment);
        }
    }
}
