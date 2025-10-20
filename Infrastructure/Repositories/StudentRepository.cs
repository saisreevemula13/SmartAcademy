using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;
    public StudentRepository(AppDbContext context) => _context = context;

    public async Task<List<Student>> GetAllAsync() =>
        await _context.Students.Include(s=>s.Enrollments).
       ThenInclude(x=>x.Course).AsNoTracking().
        ToListAsync();


    public async Task<Student> GetByIdAsync(int id) =>
        await _context.Students.Include(s => s.Enrollments).
       ThenInclude(x => x.Course).FirstOrDefaultAsync(k=>k.Id==id);

    public async Task<Student> AddAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<Student> UpdateAsync(int id, Student student)
    {
        var stud = await _context.Students.FindAsync(id);
        if (stud == null) return null;

        stud.Name = student.Name ?? stud.Name;
        stud.Email = student.Email ?? stud.Email;
        stud.PasswordHash = student.PasswordHash ?? stud.PasswordHash;

        await _context.SaveChangesAsync();
        return stud;
    }

    public async Task<Student> DeleteAsync(int id)
    {
        var stud = await _context.Students.FindAsync(id);
        if (stud == null) return null;

        _context.Students.Remove(stud);
        await _context.SaveChangesAsync();
        return stud;
    }
}
