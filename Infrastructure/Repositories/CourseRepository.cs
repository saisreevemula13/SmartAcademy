using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CourseRepository:ICourseRepository
    {
        private readonly AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.
                Include(x => x.Enrollments).
                ThenInclude(s => s.Student).AsNoTracking()
                .ToListAsync();
        }
        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.
            Include(x => x.Enrollments).
            ThenInclude(s => s.Student).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Course> AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateAsync(int id, Course course)
        {
            var existingcourse = await _context.Courses.FindAsync(id);
            if (existingcourse == null) return null;
            existingcourse.Description = course.Description?? existingcourse.Description;
            existingcourse.Name = course.Name ?? existingcourse.Name;
            return existingcourse;
        }

        public async Task<Course> DeleteAsync(int id)
        {
            var existingcourse = await _context.Courses.FindAsync(id);
            if (existingcourse == null) return null;
            _context.Courses.Remove(existingcourse);
            return existingcourse;
        }
    }
}
