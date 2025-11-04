using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace SmartAcademy.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService service, ILogger<StudentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            _logger.LogInformation("Fetching all students...");
            var students = await _service.GetAll(filterOn,filterQuery);
            _logger.LogInformation("Fetched {Count} students successfully", students.Count);
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            _logger.LogInformation("Fetching student with ID: {Id}", id);
            var student = await _service.GetById(id);
            _logger.LogInformation("Successfully fetched student {Id}", id);
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateStudDTO dto)
        {
            _logger.LogInformation("Adding new student: {Name}", dto.Name);
            var created = await _service.Add(dto);
            _logger.LogInformation("Student {Name} added successfully with ID {Id}", created.Name, created.Id);
            return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudDTO dto)
        {
            _logger.LogInformation("Updating student ID: {Id}", id);
            var updated = await _service.Update(id, dto);
            _logger.LogInformation("Student {Id} updated successfully", id);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting student ID: {Id}", id);
            var deleted = await _service.Delete(id);
            _logger.LogWarning("Student {Id} deleted", id);
            return Ok(deleted);
        }
    }
}
