using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Repositories
{
    public class CourseRepository:ICourseRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CourseRepository> _logger;
        public CourseRepository(AppDbContext context, ILogger<CourseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<Course>> GetAllAsync(string? filterOn = null, string? filterQuery = null,string? sortBy=null,
            bool? isAscending=null,
             int pageNumber = 1, int pageSize = 1000)
        {
            _logger.LogDebug("Fetching all courses from database...");

            // Start building the query
            IQueryable<Course> courseList = _context.Courses
                .Include(c => c.Enrollments)
                .ThenInclude(e => e.Student)
                .AsNoTracking();

            // Trim query parameters
            filterOn = filterOn?.Trim();
            filterQuery = filterQuery?.Trim();

            // Apply filter if filterQuery is provided
            if (!string.IsNullOrWhiteSpace(filterQuery))
            {
                if (string.IsNullOrWhiteSpace(filterOn) || filterOn.Equals("All", StringComparison.OrdinalIgnoreCase))
                {
                    // Global search on all relevant fields
                    courseList = courseList.Where(c =>
                        c.Name.Contains(filterQuery) ||
                        c.Description.Contains(filterQuery) ||
                        c.Enrollments.Any(e => e.Student.Name.Contains(filterQuery))
                    );
                }

                else if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    courseList = courseList.Where(c => c.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    courseList = courseList.Where(c => c.Description.Contains(filterQuery));
                }
                //We use any here because the enrollments is having atleast one matching record with
                //filterQuery studname then return true and we can use this condition in fetching filterd values

                else if (filterOn.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    courseList = courseList.Where(c => c.Enrollments.Any(e => e.Student.Name.Contains(filterQuery)));
                }
            }

            bool ascending = isAscending ?? true;

            // Apply sorting (default to Id descending if no sortBy provided or unrecognized)
            courseList = sortBy?.Trim().ToLower() switch
            {
                "createdat" => ascending ? courseList.OrderBy(c => c.CreatedAt)
                                         : courseList.OrderByDescending(c => c.CreatedAt),
                "name" => ascending ? courseList.OrderBy(c => c.Name)
                                    : courseList.OrderByDescending(c => c.Name),
                _ => courseList.OrderByDescending(c => c.Id)
            };

            //Pagination
            int skip = (pageNumber - 1) * pageSize;
            courseList = courseList.Skip(skip).Take(pageSize);

            _logger.LogInformation($"Page: {pageNumber}, Size: {pageSize}, SortBy: {sortBy}, Ascending: {ascending}");

            // Execute query here
            return await courseList.ToListAsync();

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
