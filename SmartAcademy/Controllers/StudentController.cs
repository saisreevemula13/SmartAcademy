using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SmartAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _service.GetAll();
            return Ok(students); // Returns a normal JSON array
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _service.GetById(id);
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateStudDTO dto)
        {
            var created = await _service.Add(dto);
            return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStudDTO dto) =>
            Ok(await _service.Update(id, dto));

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            Ok(await _service.Delete(id));
    }

}
