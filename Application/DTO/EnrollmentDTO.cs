using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class EnrollmentDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public DateTime EnrolledAt { get; set; }
    }
}
