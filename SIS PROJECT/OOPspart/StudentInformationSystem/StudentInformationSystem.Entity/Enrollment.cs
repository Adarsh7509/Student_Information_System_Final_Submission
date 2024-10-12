using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.Entity
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public DateTime EnrollmentDate { get; set; }


        public Enrollment(Student student, Course course, DateTime enrollmentDate, int enrollmentId = 0)
        {
            EnrollmentId = enrollmentId;
            Student = student;
            Course = course;
            EnrollmentDate = enrollmentDate;
        }

        public override string ToString()
        {
            return $"Enrollment ID: {EnrollmentId}, EnrollmentDate: {EnrollmentDate}";
        }
    }
}
