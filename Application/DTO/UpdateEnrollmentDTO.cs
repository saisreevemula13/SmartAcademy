using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UpdateEnrollmentDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
