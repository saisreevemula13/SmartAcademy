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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Enrollment> CreateAsync(Enrollment enrollment)
        {
            await _context.AddAsync(enrollment);
            await _context.SaveChangesAsync();
            return enrollment;
        }

        public async Task<Enrollment> DeleteByIdAsync(int id)
        {
            var enrollmentexisting = await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .FirstOrDefaultAsync(e => e.Id == id);
            if (enrollmentexisting == null) return null;
            _context.Enrollments.Remove(enrollmentexisting);
            await _context.SaveChangesAsync();
            return enrollmentexisting;
        }

        public async Task<List<Enrollment>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, Boolean? isAscending = true,
             int pageNumber = 1, int pageSize = 1000)
        {
            var enrollList = _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .AsQueryable();
            //filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("CourseId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(filterQuery, out int courseid))
                    {
                        enrollList = enrollList.Where(x => x.CourseId == courseid);
                    }
                    else
                        throw new FormatException("Invalid Course ID format.");
                }
            }
            Console.WriteLine($"sortBy: {sortBy}, isAscending: {isAscending}");
            //sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("StudentId", StringComparison.OrdinalIgnoreCase))
                {

                    enrollList = (bool)isAscending ? enrollList.OrderBy(x => x.StudentId) : enrollList.OrderByDescending(x => x.StudentId);
                }
                else if (sortBy.Equals("id", StringComparison.OrdinalIgnoreCase))
                {

                    enrollList = (bool)isAscending ? enrollList.OrderBy(x =>x.Id) : enrollList.OrderByDescending(x => x.Id);
                }
            }
            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;
            return await enrollList.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            var enrollmentexisting = await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .FirstOrDefaultAsync(e => e.Id == id);
            if (enrollmentexisting == null) return null;
            return enrollmentexisting;
        }

        public async Task<Enrollment> UpdateByIdAsync(int id, Enrollment enrollment)
        {
            var enrollmentexisting = await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .FirstOrDefaultAsync(e => e.Id == id);
            if (enrollmentexisting == null) return null;
            enrollmentexisting.Id = enrollment.Id;
            enrollmentexisting.CourseId = enrollment.CourseId;
            enrollmentexisting.StudentId = enrollment.StudentId;
            return enrollmentexisting;
        }
    }
}
