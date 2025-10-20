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

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments
                                    .Include(e => e.Student)
                                    .Include(e => e.Course)
                                    .ToListAsync();
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
            enrollmentexisting.Id= enrollment.Id;
            enrollmentexisting.CourseId=enrollment.CourseId;
            enrollmentexisting.StudentId = enrollment.StudentId;
            return enrollmentexisting;
        }
    }
}
