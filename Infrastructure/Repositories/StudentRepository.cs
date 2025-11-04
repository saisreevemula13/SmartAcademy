using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentRepository> _logger;

    public StudentRepository(AppDbContext context, ILogger<StudentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Student>> GetAllAsync(string? filterOn,string? filterQuery)
    {
        _logger.LogDebug("Fetching all students from database...");
        var studList=  _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(x => x.Course)
            .AsNoTracking()
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
        {
            if (filterOn.Equals(("Name"), StringComparison.OrdinalIgnoreCase))
            {
                studList=studList.Where(x=>x.Name.Contains(filterQuery));
            }
        }
        return await studList.ToListAsync();
    }

    public async Task<Student> GetByIdAsync(int id)
    {
        _logger.LogDebug("Fetching student with ID {Id} from database", id);
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(x => x.Course)
            .FirstOrDefaultAsync(k => k.Id == id);

        if (student == null)
            _logger.LogWarning("Student with ID {Id} not found in database", id);

        return student;
    }

    public async Task<Student> AddAsync(Student student)
    {
        try
        {
            _logger.LogDebug("Adding student {Name} to database", student.Name);
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Student {Name} saved to database with ID {Id}", student.Name, student.Id);
            return student;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding student {Name} to database", student.Name);
            throw;
        }
    }

    public async Task<Student> UpdateAsync(int id, Student student)
    {
        _logger.LogDebug("Updating student {Id} in database", id);
        var stud = await _context.Students.FindAsync(id);

        if (stud == null)
        {
            _logger.LogWarning("Student {Id} not found for update", id);
            return null;
        }

        stud.Name = student.Name ?? stud.Name;
        stud.Email = student.Email ?? stud.Email;
        stud.PasswordHash = student.PasswordHash ?? stud.PasswordHash;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Student {Id} updated successfully in database", id);
        return stud;
    }

    public async Task<Student> DeleteAsync(int id)
    {
        _logger.LogDebug("Deleting student {Id} from database", id);
        var stud = await _context.Students.FindAsync(id);

        if (stud == null)
        {
            _logger.LogWarning("Student {Id} not found for delete", id);
            return null;
        }

        _context.Students.Remove(stud);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Student {Id} deleted from database", id);
        return stud;
    }
}
