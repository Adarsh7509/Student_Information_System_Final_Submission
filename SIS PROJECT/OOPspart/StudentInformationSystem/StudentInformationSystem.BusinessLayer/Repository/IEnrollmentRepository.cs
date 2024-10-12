using StudentInformationSystem.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentInformationSystem.BusinessLayer.Repository
{
    public interface IEnrollmentRepository
    {
        Enrollment GetEnrollmentById(int id);
        IEnumerable<Enrollment> GetAllEnrollments();
        int AddEnrollment(Enrollment enrollment);
        void UpdateEnrollment(Enrollment enrollment);
        void DeleteEnrollment(int id);
        bool Exists(int studentId, int courseId);
    }
}
