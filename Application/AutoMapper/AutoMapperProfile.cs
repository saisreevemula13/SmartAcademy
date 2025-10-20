using Application.DTO;
using Domain.Entities;
using AutoMapper;

namespace Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // For create / update
            CreateMap<Course, CourseDTO>()
     .ForMember(dest => dest.Students,
                opt => opt.MapFrom(src =>
                    src.Enrollments.Select(e => e.Student)));
            CreateMap<Student, StudentInCourseDTO>();

            // 2️⃣ Updated: custom mapping for Student → StudentDTO
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Courses,
                           opt => opt.MapFrom(src =>
                               src.Enrollments != null
                                   ? src.Enrollments.Select(e => e.Course)
                                   : new List<Course>()));
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<UpdateCourseDTO, Course>();
            CreateMap<CreateStudDTO, Student>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));

            CreateMap<UpdateStudDTO, Student>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password))
                .ForAllMembers(opt => opt.Condition((src, dest, srcValue) => srcValue != null));

            // Map Student -> StudentDTO for GET endpoints

            
            CreateMap<Student, StudentDTO>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Enrollments.Select(s => s.Course)));
          

            CreateMap<Enrollment, EnrollmentDTO>();
            CreateMap<CreateEnrollmentDTO, Enrollment>();
            CreateMap<UpdateEnrollmentDTO, Enrollment>();
        }
    }
}
