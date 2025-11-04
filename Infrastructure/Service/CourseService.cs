using Application.DTO;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Service
{
    public class CourseService:ICourseService
    {
            private readonly ICourseRepository _repository;
            private readonly IMapper _mapper;

            public CourseService(ICourseRepository repo, IMapper mapper)
            {
                _repository = repo;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CourseDTO>> GetAll(string? filterOn = null, string? filterQuery = null,
                string? sortBy=null, bool? isAscending=null,
                 int pageNumber = 1,int pageSize = 1000)
            {
                var courses = await _repository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending);
                return _mapper.Map<IEnumerable<CourseDTO>>(courses);
            }

            public async Task<CourseDTO> GetById(int id)
            {

                var course = await _repository.GetByIdAsync(id);
                if (course == null)
                    throw new NotFoundException($"Course with ID {id} not found");

                return _mapper.Map<CourseDTO>(course);
            }

            public async Task<CourseDTO> Add(CreateCourseDTO dto)
            {
                ArgumentNullException.ThrowIfNull(dto);

                var course = _mapper.Map<Course>(dto);

                course = await _repository.AddAsync(course);
                return _mapper.Map<CourseDTO>(course);
            }

            public async Task<CourseDTO> Update(int id, UpdateCourseDTO dto)
            {
                // hello
                var course = _mapper.Map<Course>(dto);
                course = await _repository.UpdateAsync(id, course)
                    ?? throw new NotFoundException($"Course with ID {id} not found to update.");

                return _mapper.Map<CourseDTO>(course);
            }

            public async Task<CourseDTO> Delete(int id)
            {

                var course = await _repository.DeleteAsync(id)
               ?? throw new NotFoundException($"Course with ID {id} not found to delete.");

                return _mapper.Map<CourseDTO>(course);
            }
    }
}



