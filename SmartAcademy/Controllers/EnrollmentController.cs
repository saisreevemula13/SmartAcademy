using Application.DTO;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SmartAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService service)
        {
            _enrollmentService = service;
        }

        // GET: api/enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EnrollmentDTO>>> GetAll()
        {
            var enrollments = await _enrollmentService.GetAllEnrollments();
            return Ok(enrollments);
        }

        // GET: api/enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> GetById(int id)
        {
            var enrollment = await _enrollmentService.GetEnrollmentById(id);
            if (enrollment == null)
                return NotFound();
            return Ok(enrollment);
        }

        // POST: api/enrollments
        [HttpPost]
        public async Task<ActionResult<EnrollmentDTO>> Create(CreateEnrollmentDTO enrollmentDTO)
        {
            var created = await _enrollmentService.CreateEnrollment(enrollmentDTO);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/enrollments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> Update(int id, UpdateEnrollmentDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("Id in URL does not match DTO");

            var updated = await _enrollmentService.UpdateEnrollment(id,dto);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/enrollments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EnrollmentDTO>> Delete(int id)
        {
            var deleted = await _enrollmentService.DeleteById(id);
            if (deleted == null)
                return NotFound();

            return Ok(deleted);
        }
    }

}
