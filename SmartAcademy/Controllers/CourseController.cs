using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SmartAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
             private readonly ICourseService _service;

            public CourseController(ICourseService service) => _service = service;

            [HttpGet]
            public async Task<IActionResult> GetAllCourses()
            {
                var courses = await _service.GetAll();
                return Ok(courses); // Returns a normal JSON array
            }

            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetCourseById(int id)
            {
                var course = await _service.GetById(id);
                return Ok(course);
            }

            [HttpPost]
            public async Task<IActionResult> Add([FromBody] CreateCourseDTO dto)
            {
                var created = await _service.Add(dto);
                return CreatedAtAction(nameof(GetCourseById), new { id = created.Id }, created);
            }

            [HttpPut("{id:int}")]
            public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDTO dto) =>
                Ok(await _service.Update(id, dto));

            [HttpDelete("{id:int}")]
            public async Task<IActionResult> Delete(int id) =>
                Ok(await _service.Delete(id));
    }

}

